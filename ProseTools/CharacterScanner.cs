using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Python.Runtime;
using opennlp.tools.namefind;
using opennlp.tools.tokenize;
using opennlp.tools.util;
using com.sun.tools.javac.parser;
using com.sun.imageio.plugins.common;
using System.IO.Pipes;



namespace ProseTools
{
    internal class CharacterScanner
    {
        // Threshold for chunking text (number of words)
        private const int ChunkWordThreshold = 10000;

        // Static spaCy NLP model (loaded only once)
        private static dynamic _spacyNlp = null;

        // Words that we never consider as part of a proper name.
        private readonly System.Collections.Generic.HashSet<string> _excludedWords = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "The", "A", "An", "And", "But", "Or", "Nor", "Yet", "So", "At", "By",
            "For", "In", "Of", "On", "To", "With", "Is", "Are", "Was", "Were"
        };

        // Titles that may indicate a proper name sequence.
        private readonly System.Collections.Generic.HashSet<string> _titles = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Prof.", "Dr.", "Mr.", "Mrs.", "Ms.", "Sr.", "Sra.", "Miss."
        };

        // Progress-tracking object. (Assumed to be defined elsewhere.)
        private WordParsingCount wordParsingCount;

        /// <summary>
        /// Scans the provided Word document for sequences of words that look like names.
        /// </summary>
        /// <param name="doc">The Word document to scan.</param>
        /// <param name="ignoreFirstWordInSentence">
        /// If true, the first word in each sentence is ignored (to avoid false positives).
        /// </param>
        /// <returns>A deduplicated list of potential names found in the document.</returns>
        public List<string> ScanForNames(Document doc, bool ignoreFirstWordInSentence)
        {
            var potentialNames = new List<string>();
            var currentNameParts = new List<string>();
            bool inNameSequence = false;
            bool skipNextWord = false; // Used to skip the first word of a sentence if required

            // Retrieve the entire document text at once.
            string documentText = doc.Content.Text;

            // Use a regex to split the text into words (splitting on whitespace).
            string[] words = Regex.Split(documentText, @"\s+");

            int totalWords = words.Length;
            int currentIndex = 0;

            // Initialize and display the progress tracker.
            wordParsingCount = new WordParsingCount();
            wordParsingCount.InitializeCount(totalWords);
            wordParsingCount.Show();

            // For progress updates (update every ~1% of the words processed)
            int updateFrequency = Math.Max(1, totalWords / 100);

            foreach (string rawWord in words)
            {
                currentIndex++;
                if (currentIndex % updateFrequency == 0)
                {
                    wordParsingCount.SetWordCountParsingText(currentIndex, totalWords);
                }

                // Clean the word (trim and remove trailing punctuation).
                string word = CleanWord(rawWord);
                if (string.IsNullOrEmpty(word))
                    continue;

                // Determine if this word is at the start of a sentence.
                bool isStartOfSentence = IsStartOfSentence(words, currentIndex);

                // Optionally skip the first word in the sentence.
                if (ignoreFirstWordInSentence && skipNextWord)
                {
                    skipNextWord = false;
                    continue;
                }
                if (isStartOfSentence)
                {
                    skipNextWord = ignoreFirstWordInSentence;
                }

                // If the word is a recognized title and we're not already in a name sequence, start a new sequence.
                if (currentNameParts.Count == 0 && _titles.Contains(word))
                {
                    inNameSequence = true;
                    currentNameParts.Add(word);
                    continue;
                }

                // Check if the word qualifies as a potential part of a name.
                if (QualifiesAsName(word, isStartOfSentence))
                {
                    inNameSequence = true;
                    currentNameParts.Add(word);
                }
                else
                {
                    // If we were in a name sequence, flush it.
                    if (inNameSequence && currentNameParts.Count > 0)
                    {
                        potentialNames.Add(string.Join(" ", currentNameParts));
                        currentNameParts.Clear();
                    }
                    inNameSequence = false;
                }
            }

            // Flush any remaining name sequence.
            if (currentNameParts.Count > 0)
            {
                potentialNames.Add(string.Join(" ", currentNameParts));
            }

            wordParsingCount.SetWordCountParsingText("Processing duplicates...");
            var uniqueNames = new System.Collections.Generic.HashSet<string>(potentialNames, StringComparer.OrdinalIgnoreCase);
            wordParsingCount.Close();

            return uniqueNames.ToList();
        }

        /// <summary>
        /// Removes trailing punctuation from a word and trims whitespace.
        /// </summary>
        private string CleanWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return string.Empty;

            // Remove common trailing punctuation: period, comma, question mark, exclamation, semicolon, colon.
            return Regex.Replace(word.Trim(), @"[.,!?;:]+$", string.Empty);
        }

        /// <summary>
        /// Determines if the current word is at the start of a sentence.
        /// Uses the previous word (if any) to decide if a sentence-ending punctuation was present.
        /// </summary>
        private bool IsStartOfSentence(string[] words, int currentIndex)
        {
            // currentIndex is 1-based in our loop; adjust to zero-based index.
            int idx = currentIndex - 1;
            if (idx <= 0)
                return false;

            string previousWord = words[idx - 1];
            if (string.IsNullOrEmpty(previousWord))
                return false;

            char lastChar = previousWord.Trim().LastOrDefault();
            return lastChar == '.' || lastChar == '?' || lastChar == '!';
        }

        /// <summary>
        /// Determines whether a word qualifies as part of a name.
        /// A word qualifies if its first character is uppercase and,
        /// if it’s at the start of a sentence, it is not in the excluded list.
        /// </summary>
        private bool QualifiesAsName(string word, bool isStartOfSentence)
        {
            if (string.IsNullOrEmpty(word))
                return false;

            // Must start with an uppercase letter.
            if (!char.IsUpper(word[0]))
                return false;

            // If it's at the start of a sentence, exclude common words.
            if (isStartOfSentence && _excludedWords.Contains(word))
                return false;

            return true;
        }


        /// <summary>
        /// Initializes the spaCy NLP model if it isn't already loaded.
        /// </summary>
        private void InitializeSpacyModel()
        {
            if (_spacyNlp == null)
            {
                using (Py.GIL())
                {
                    dynamic spacy = Py.Import("spacy");
                    _spacyNlp = spacy.load("en_core_web_sm");


                }
            }
        }

        /// <summary>
        /// Splits text into chunks based on a maximum number of words.
        /// </summary>
        private List<string> SplitTextIntoChunks(string text, int chunkWordCount)
        {
            var words = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();

            for (int i = 0; i < words.Length; i += chunkWordCount)
            {
                var chunk = string.Join(" ", words.Skip(i).Take(chunkWordCount));
                chunks.Add(chunk);
            }

            return chunks;
        }

        /// <summary>
        /// Processes a single text chunk using the preloaded spaCy model.
        /// Returns a list of person names found in the chunk.
        /// </summary>
        private List<string> ProcessChunk(string textChunk)
        {
            var names = new List<string>();

            using (Py.GIL())
            {
                try
                {
                    dynamic doc = _spacyNlp(textChunk);
                    foreach (dynamic ent in doc.ents)
                    {
                        if (ent.label_.ToString() == "PERSON")
                        {
                            string personName = ent.text.ToString();
                            if (!names.Contains(personName, StringComparer.OrdinalIgnoreCase))
                            {
                                names.Add(personName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in spaCy processing: {ex.Message}");
                }
            }

            return names;
        }

        /// <summary>
        /// Scans the provided text for person names using OpenNLP.
        /// Splits the text into chunks if necessary, tokenizes each chunk, and applies name finding.
        /// </summary>
        /// 
        public List<string> ScanForNamesUsingOpenNLP(string text)
        {
            // Split the text into words.
            var allWords = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Split the text into chunks if there are too many words.
            List<string> chunks = (allWords.Length > ChunkWordThreshold)
                ? SplitTextIntoChunks(text, ChunkWordThreshold)
                : new List<string> { text };

            var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Build relative path for the name finder model.
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelFolder = Path.Combine(baseDir, "NLPModels");
            string nameFinderModelPath = Path.Combine(modelFolder, "en-ner-person.bin");

            if (!File.Exists(nameFinderModelPath))
            {
                MessageBox.Show("Model file not found. Please ensure en-ner-person.bin is in the NLPModels folder.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return allNames.ToList();
            }

            using (var modelStream = new FileStream(nameFinderModelPath, FileMode.Open, FileAccess.Read))
            {
                // Wrap the FileStream as a java.io.InputStream.
                java.io.InputStream javaStream = new ikvm.io.InputStreamWrapper(modelStream);
                var model = new TokenNameFinderModel(javaStream);
                var nameFinder = new NameFinderME(model);

                // Process each chunk.
                foreach (string chunk in chunks)
                {
                    // Convert the chunk to tokens.
                    string[] tokens = chunk.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    // Find name spans in this token array.
                    Span[] nameSpans = nameFinder.find(tokens);
                    foreach (var span in nameSpans)
                    {
                        // Get start and end indices.
                        int start = span.getStart();
                        int end = span.getEnd(); // 'end' is exclusive.
                                                 // Join the tokens for this span with a space between them.
                        string name = string.Join(" ", tokens.Skip(start).Take(end - start));
                        if (!string.IsNullOrEmpty(name))
                        {
                            allNames.Add(name.Trim());
                        }
                    }
                }
            }

            return allNames.ToList();
        }

        public List<string> ScanForNamesUsingSpacy(string text)  // This function has to be fitted with your code for spaCy to work right ok
        {
            int x = -1;

            if (x > 0)
            {
                MessageBox.Show("SpaCy is not available in this version of the software.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<string>();
            }
                //return System.Threading.Tasks.Task.Run(() =>
            {
                var allNames = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

                try
                {
                    // Correct order: Initialize first, then import sys.
                    if (!PythonEngine.IsInitialized)
                    {
                        //Console.Write("Hello, PythonNET!");  //There is no console in Windows Forms :-D
                        PythonEngine.Initialize();
                    }

                    using (Py.GIL())
                    {
                        dynamic sys = Py.Import("sys");
                        sys.path.append("C:\\Users\\username\\AppData\\Local\\Programs\\Python\\Python38\\Lib\\site-packages");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error initializing Python engine: {e.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return allNames.ToList();
                }

                // Load the spaCy model if necessary.
                InitializeSpacyModel();

                // Determine if we need to split the text into chunks.
                var allWords = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> chunks;
                if (allWords.Length > ChunkWordThreshold)
                {
                    chunks = SplitTextIntoChunks(text, ChunkWordThreshold);
                }
                else
                {
                    chunks = new List<string> { text };
                }

                // Process each chunk and accumulate names.
                foreach (var chunk in chunks)
                {
                    var namesFromChunk = ProcessChunk(chunk);
                    foreach (var name in namesFromChunk)
                    {
                        allNames.Add(name);
                    }
                }

                return allNames.ToList();
            }//);
        }
    }


}
