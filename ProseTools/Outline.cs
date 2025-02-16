using edu.stanford.nlp.ling;
using ProseTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;

internal class Outline
{
    // The Book node is the designated root of the outline.
    private OutlineNode rootNode;

    public OutlineNode RootNode => rootNode;

    public Outline() { }

    /// <summary>
    /// Initializes the outline with a Book node as the root.
    /// </summary>
    public void InitializeRoot(OutlineNode bookNode)
    {
        if (bookNode == null)
            throw new ArgumentNullException(nameof(bookNode), "Book node cannot be null.");

        rootNode = bookNode;
    }

    /// <summary>
    /// Adds a chapter node as a child of the Book root.
    /// </summary>
    public void AddChapter(OutlineNode chapterNode)
    {
        if (rootNode == null)
            throw new InvalidOperationException("Outline has no root node. Call InitializeRoot() first.");
        if (chapterNode == null)
            throw new ArgumentNullException(nameof(chapterNode), "Chapter node cannot be null.");

        // Optionally, you might check for duplicates here:
        if (FindNode(chapterNode.Title) == null)
        {
            rootNode.AddChild(chapterNode);
        }
    }

    /// <summary>
    /// Finds a node anywhere in the outline by title.
    /// </summary>
    public OutlineNode FindNode(string title)
    {
        if (rootNode == null)
            return null;
        return FindNodeRecursive(rootNode, title);
    }

    private OutlineNode FindNodeRecursive(OutlineNode current, string title)
    {
        if (current.Title == title)
            return current;
        foreach (var child in current.Children)
        {
            var found = FindNodeRecursive(child, title);
            if (found != null)
                return found;
        }
        return null;
    }

    /// <summary>
    /// Adds a node to a specific parent node identified by title.
    /// </summary>
    public bool AddNodeToParent(string parentTitle, OutlineNode childNode)
    {
        var parentNode = FindNode(parentTitle);
        if (parentNode != null)
        {
            // Optionally check for duplicates:
            if (parentNode.FindChild(childNode.Title) == null)
            {
                parentNode.AddChild(childNode);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Serializes the outline to XML.
    /// </summary>
    public XElement ToXML()
    {
        XNamespace ns = ProseMetaData.MetadataNamespace;
        if (rootNode != null)
        {
            // Serialize the single root node
            return rootNode.ToXML();
        }
        else
        {
            // Return an empty Outline element in the proper namespace.
            return new XElement(ns + "Outline");
        }
    }


    /// <summary>
    /// Deserializes the outline from XML.
    /// </summary>
    public static Outline FromXML(XElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

        // Assume the root node is the first "Node" element.
        var rootElement = element.Element("Node");
        if (rootElement == null)
            throw new InvalidOperationException("No root node found in XML.");

        Outline outline = new Outline();
        outline.rootNode = OutlineNode.FromXML(rootElement);
        return outline;
    }

    /// <summary>
    /// Scans the provided Word document for paragraphs styled as "Prose Chapter" or "Heading 1" and
    /// adds new chapters as children of the root node. Returns the number of chapters added.
    /// </summary>
    /// <param name="doc">The Word document to scan.</param>
    /// <returns>The count of new chapters added.</returns>
    public int Scan(Word.Document doc)
    {
        if (doc == null)
            throw new ArgumentNullException(nameof(doc));

        if (this.RootNode == null)
            throw new InvalidOperationException("Outline has no root node. Please initialize the outline first.");

        int addedCount = 0;

        foreach (Word.Paragraph para in doc.Paragraphs)
        {
            var style = para.get_Style() as Word.Style;
            if (style != null &&
                (style.NameLocal.Equals("Prose Chapter", StringComparison.CurrentCultureIgnoreCase) ||
                 style.NameLocal.Equals("Heading 1", StringComparison.CurrentCultureIgnoreCase)))
            {
                string chapterTitle = para.Range.Text.Trim();

                // Exclude Table of Contents entries.
                if (chapterTitle.StartsWith("Table of Contents", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (!string.IsNullOrWhiteSpace(chapterTitle))
                {
                    // Check for duplicates among the root node's children.
                    if (this.RootNode.FindChild(chapterTitle) == null)
                    {
                        OutlineNode chapterNode = new OutlineNode
                        {
                            Title = chapterTitle,
                            Details = "Double-click to edit details or add notes.",
                            Notes = string.Empty
                        };

                        this.RootNode.AddChild(chapterNode);
                        addedCount++;
                    }
                }
            }
        }
        return addedCount;
    }
}


internal class OutlineNode
{
    public string Title { get; set; }
    public string Details { get; set; }
    public string Notes { get; set; } // New field for additional notes.
    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    private List<OutlineNode> children = new List<OutlineNode>();

    public IReadOnlyList<OutlineNode> Children => children.AsReadOnly();

    // Add a child node
    public void AddChild(OutlineNode child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child), "Child node cannot be null.");

        children.Add(child);
    }

    // Delete a child node by title
    public bool DeleteChild(string title)
    {
        var child = children.FirstOrDefault(c => c.Title == title);
        if (child != null)
        {
            children.Remove(child);
            return true;
        }
        return false;
    }

    // Find a child node by title
    public OutlineNode FindChild(string title)
    {
        return FindChildRecursive(children, title);
    }

    // Serialize to XML
    public XElement ToXML()
    {
        XNamespace ns = ProseMetaData.MetadataNamespace;
        return new XElement(ns + "Node",
            new XElement(ns + "Title", Title),
            new XElement(ns + "Details", Details),
            new XElement(ns + "Notes", Notes),
            new XElement(ns + "Attributes",
                Attributes.Select(attr => new XElement(ns + attr.Key, attr.Value))
            ),
            new XElement(ns + "Children",
                children.Select(child => child.ToXML())
            )
        );
    }

    // Deserialize from XML
    public static OutlineNode FromXML(XElement element)
    {
        var node = new OutlineNode
        {
            Title = element.Element("Title")?.Value,
            Details = element.Element("Details")?.Value,
            Notes = element.Element("Notes")?.Value
        };

        var attributes = element.Element("Attributes");
        if (attributes != null)
        {
            node.Attributes = attributes.Elements().ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
        }

        var children = element.Element("Children");
        if (children != null)
        {
            node.children = children.Elements("Node").Select(FromXML).ToList();
        }
        return node;
    }

    private OutlineNode FindChildRecursive(IEnumerable<OutlineNode> children, string title)
    {
        foreach (var child in children)
        {
            if (child.Title == title)
                return child;

            var found = FindChildRecursive(child.Children, title);
            if (found != null)
                return found;
        }
        return null;
    }

}

