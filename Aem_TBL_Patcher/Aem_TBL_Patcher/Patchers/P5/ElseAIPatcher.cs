using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class ElseAIPatcher : AutoBasePatcher
    {
        public ElseAIPatcher() : base("ELSAI", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(44, "ELSEAI Segment 1"),
            new Segment(320, "ELSEAI Segment 2")
        };
    }
}