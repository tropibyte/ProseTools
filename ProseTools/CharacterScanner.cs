using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ProseTools
{
    internal class CharacterScanner
    {

        private readonly HashSet<string> _excludedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "The", "A", "An", "And", "But", "Or", "Nor", "Yet", "So", "At", "By",
                "For", "In", "Of", "On", "To", "With", "Is", "Are", "Was", "Were"
            };

        private WordParsingCount wordParsingCount;

        public List<string> ScanForNames(Microsoft.Office.Interop.Word.Document doc, bool bIgnoreFirstWord)
        {
            var potentialNames = new List<string>();
            var currentName = new List<string>();
            bool inNameSequence = false;
            bool skipNextWord = false; // Flag to skip the first word of the next sentence

            // List of recognized titles
            var titles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Prof.", "Dr.", "Mr.", "Mrs.", "Ms.", "Sr.", "Sra.", "Miss."
    };

            // Extract the entire document text in one interop call
            string documentText = doc.Content.Text;

            // Split text into words using Regex for robustness
            var words = Regex.Split(documentText, @"\s+");

            int nCount = words.Length, nIndex = 0;
            wordParsingCount = new WordParsingCount();
            wordParsingCount.InitializeCount(nCount);
            wordParsingCount.Show();

            int updateFrequency = (int)Math.Max(1.0, nCount / 1000.0);

            foreach (string word in words)
            {
                // Increment word index for progress tracking
                ++nIndex;

                if (nIndex % updateFrequency == 0) // Update progress less frequently
                {
                    wordParsingCount.SetWordCountParsingText(nIndex, nCount);
                }

                // Skip empty or whitespace-only words
                string text = word.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                // Remove trailing punctuation (., ? ! etc.)
                text = Regex.Replace(text, @"[.,!?;:]+$", string.Empty);

                // Check if the previous character indicates a new sentence
                bool isStartOfSentence = IsStartOfSentence(text, nIndex > 1 ? words[nIndex - 2] : string.Empty);

                // Skip the first word of the sentence if the flag is set
                if (bIgnoreFirstWord && skipNextWord)
                {
                    skipNextWord = false; // Reset the flag after skipping
                    continue;
                }

                if (isStartOfSentence)
                {
                    skipNextWord = bIgnoreFirstWord; // Set the flag to skip the next word
                }

                // Handle concatenation for titles
                if (currentName.Count == 0 && titles.Contains(text))
                {
                    inNameSequence = true;
                    currentName.Add(text);
                    continue;
                }

                // Check if it's a capitalized word and (if at the start of a sentence) not in the excluded list
                if (char.IsUpper(text[0]) && (!isStartOfSentence || !_excludedWords.Contains(text)))
                {
                    inNameSequence = true;
                    currentName.Add(text);
                }
                else
                {
                    // End of a name sequence
                    if (inNameSequence && currentName.Count > 0)
                    {
                        potentialNames.Add(string.Join(" ", currentName));
                        currentName.Clear();
                    }
                    inNameSequence = false;
                }
            }

            // Add any remaining name at the end
            if (currentName.Count > 0)
            {
                potentialNames.Add(string.Join(" ", currentName));
            }

            wordParsingCount.SetWordCountParsingText("Processing duplicates...");

            // Deduplicate and finalize
            var uniqueNames = new HashSet<string>(potentialNames, StringComparer.OrdinalIgnoreCase);
            wordParsingCount.Close();
            //wordParsingCount.SetWordCountParsingText("Done!");

            return uniqueNames.ToList();
        }

        // Helper method to determine if a word is at the start of a sentence
        private bool IsStartOfSentence(string word, string previousWord)
        {
            // Check if the previous word ends with a sentence-ending punctuation
            if (!string.IsNullOrEmpty(previousWord))
            {
                char lastChar = previousWord[previousWord.Length - 1];
                return lastChar == '.' || lastChar == '?' || lastChar == '!';
            }
            return false;
        }

    }
}
