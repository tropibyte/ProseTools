using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProseTools
{

    /// <summary>
    /// Null Object pattern for metadata, used when no metadata exists.
    /// </summary>
    internal class NullMetaData : ProseMetaData
    {
        public override void ReadFromActiveDocument() { /* Do nothing */ }
        public override void WriteToActiveDocument() { /* Do nothing */ }
    }
}
