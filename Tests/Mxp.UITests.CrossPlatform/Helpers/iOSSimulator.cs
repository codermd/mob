using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mxp.UITests.CrossPlatform.Helpers
{
    class iOSSimulator
    {
        public iOSSimulator(string line)
        {
            ParseLine(line);
        }

        public string Line { get; private set; }

        public string GUID { get; private set; }

        public string Name { get; private set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(GUID) && !(string.IsNullOrWhiteSpace(Name));
        }

        public override string ToString()
        {
            return Line;
        }

        void ParseLine(string line)
        {

            GUID = string.Empty;
            Name = string.Empty;
            Line = string.Empty;

            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }
            Line = line.Trim();
            var idx1 = line.IndexOf(" [");
            if (idx1 < 1)
            {
                return;
            }

            Name = Line.Substring(0, idx1).Trim();
            GUID = Line.Substring(idx1 + 2, 36).Trim();
        }
    }
}
