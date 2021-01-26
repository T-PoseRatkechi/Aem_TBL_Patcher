using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class EncountPatcher : AutoBasePatcher
    {
        public EncountPatcher() : base("ENCOUNT", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(28, "Encounter")
        };
    }
}