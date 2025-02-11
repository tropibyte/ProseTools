using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class Outline
{
    private List<OutlineNode> nodes = new List<OutlineNode>();

    public IReadOnlyList<OutlineNode> Nodes => nodes.AsReadOnly();

    // Add a node to the root of the outline
    public void AddNode(OutlineNode node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node cannot be null.");

        nodes.Add(node);
    }

    // Delete a node from the root by title
    public bool DeleteNode(string title)
    {
        var node = nodes.FirstOrDefault(n => n.Title == title);
        if (node != null)
        {
            nodes.Remove(node);
            return true;
        }
        return false;
    }

    // Find a node by title
    public OutlineNode FindNode(string title)
    {
        return FindNodeRecursive(nodes, title);
    }

    // Add a node to a specific parent node
    public bool AddNodeToParent(string parentTitle, OutlineNode childNode)
    {
        var parentNode = FindNode(parentTitle);
        if (parentNode != null)
        {
            parentNode.AddChild(childNode);
            return true;
        }
        return false;
    }

    // Serialize to XML
    public XElement ToXML()
    {
        return new XElement("Outline", nodes.Select(node => node.ToXML()));
    }

    // Deserialize from XML
    public static Outline FromXML(XElement element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

        var outline = new Outline();
        outline.nodes = element.Elements("Node").Select(OutlineNode.FromXML).ToList();
        return outline;
    }

    private OutlineNode FindNodeRecursive(IEnumerable<OutlineNode> nodes, string title)
    {
        foreach (var node in nodes)
        {
            if (node.Title == title)
                return node;

            var found = FindNodeRecursive(node.Children, title);
            if (found != null)
                return found;
        }
        return null;
    }
}

internal class OutlineNode
{
    public string Title { get; set; }
    public string Details { get; set; }
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
        return new XElement("Node",
            new XElement("Title", Title),
            new XElement("Details", Details),
            new XElement("Attributes", Attributes.Select(attr => new XElement(attr.Key, attr.Value))),
            new XElement("Children", children.Select(child => child.ToXML()))
        );
    }

    // Deserialize from XML
    public static OutlineNode FromXML(XElement element)
    {
        var node = new OutlineNode
        {
            Title = element.Element("Title")?.Value,
            Details = element.Element("Details")?.Value
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
