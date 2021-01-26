using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class AicalcPatcher : AutoBasePatcher
    {
        public AicalcPatcher() : base("AICALC", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(164, "AICALC Segment 0"),
            new Segment(4, "AICALC Segment 1"),
            new Segment(4, "AICALC Segment 2"),
            new Segment(4, "AICALC Segment 3"),
            new Segment(4, "AICALC Segment 4"),
            new Segment(3, "AICALC Segment 5"),
            new Segment(4, "AICALC Segment 6")
        };
    }
}