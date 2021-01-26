using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class SkillPatcher : AutoBasePatcher
    {
        public SkillPatcher() : base("SKILL", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(2, "Skill Elements"),
            new Segment(44, "Active Skill Data")
        };
    }
}
