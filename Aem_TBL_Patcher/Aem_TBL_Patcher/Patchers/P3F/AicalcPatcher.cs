using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class AicalcPatcher : AutoBasePatcher
    {
        public AicalcPatcher() : base("AICALC", false) { }

        protected override Segment[] Segments => new Segment[]
{
            new Segment("AIUnknown1"),
            new Segment("AIUnknown2"),
            new Segment("AIUnknown3"),
            new Segment("AIUnknown4"),
            new Segment("AIUnknown5"),
            new Segment("AIUnknown6"),
            new Segment("AIUnknown7"),
            new Segment("AIUnknown8"),
            new Segment("AIUnknown9"),
            new Segment("AIUnknown10"),
            new Segment("AIUnknown11"),
            new Segment("AIUnknown12"),
            new Segment("AIUnknown13"),
            new Segment("AIUnknown14"),
            new Segment("AIUnknown15"),
            new Segment("AIUnknown16"),
            new Segment("PlayerAIFlowscript"),
            new Segment("EnemyAIFlowscript")
        };
    }
}