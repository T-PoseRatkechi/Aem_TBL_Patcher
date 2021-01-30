using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class AicalcFPatcher : AutoBasePatcher
    {
        public AicalcFPatcher() : base("AICALC_F", false) { }

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
            new Segment(0, "AIUnknown10"),
            new Segment(0, "AIUnknown11"),
            new Segment(0, "AIUnknown12"),
            new Segment(0, "AIUnknown13"),
            new Segment(0, "AIUnknown14"),
            new Segment(0, "AIUnknown15"),
            new Segment(0, "AIUnknown16"),
            new Segment(0, "PlayerAIFlowscript"),
            new Segment(0, "EnemyAIFlowscript")
        };
    }
}