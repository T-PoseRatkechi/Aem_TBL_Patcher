using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class EncountPatcher : AutoBasePatcher
    {
        public EncountPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes, "ENCOUNT", false) { }

        protected override Segment[] Segments => new Segment[] {
            new Segment(24, "Encounter")
        };
    }
}
