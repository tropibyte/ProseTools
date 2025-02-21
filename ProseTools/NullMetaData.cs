﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{

    /// <summary>
    /// Null Object pattern for metadata, used when no metadata exists.
    /// </summary>
    internal class NullMetaData : ProseMetaData
    {
        public static readonly string Name = "Null";
        public override string _name => Name;

        public override void ReadFromActiveDocument() { /* Do nothing */ }
        public override void WriteToActiveDocument() { /* Do nothing */ }
        public override XElement ToXML()
        {
            // Return a minimal XML element indicating no metadata
            XNamespace ns = MetadataNamespace;
            return new XElement(ns + "ProseMetaData",
                new XElement(ns + "ProseType", Name)
            );
        }

        // Optionally, you could add a static method for FromXML if needed.
        public static NullMetaData FromXML(XElement element)
        {
            return new NullMetaData();
        }
    }
}
