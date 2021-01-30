using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class AicalcPatcher : AutoBasePatcher
    {
        public AicalcPatcher() : base("AICALC", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(0, "AIUnknown1"),
            new Segment(0, "AIUnknown2"),
            new Segment(0, "AIUnknown3"),
            new Segment(0, "AIUnknown4"),
            new Segment(0, "AIUnknown5"),
            new Segment(0, "AIUnknown6"),
            new Segment(0, "AIUnknown7"),
            new Segment(0, "AIUnknown8"),
            new Segment(0, "AIUnknown9"),
            new Segment(0, "PlayerAIFlowscript"),
            new Segment(0, "EnemyAIFlowscript")
        };
    }
}
