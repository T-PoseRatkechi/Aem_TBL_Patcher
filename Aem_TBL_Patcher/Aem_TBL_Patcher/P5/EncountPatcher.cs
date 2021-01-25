using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class EncountPatcher : AutoBasePatcher
    {
        public EncountPatcher() : base("ENCOUNT", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(24, "Encounter")
        };
    }
}