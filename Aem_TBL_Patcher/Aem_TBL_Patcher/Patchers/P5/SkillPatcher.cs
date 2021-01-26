using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class SkillPatcher : AutoBasePatcher
    {
        public SkillPatcher() : base("SKILL", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(3, "Skill Elements"),
            new Segment(44, "Skill Data"),
        };
    }
}