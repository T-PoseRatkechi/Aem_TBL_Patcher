using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class SkillPatcher : AutoBasePatcher
    {
        public SkillPatcher() : base("SKILL", false) { }

        protected override Segment[] Segments => new Segment[] {
            new Segment(2, "Elements"),
            new Segment(44, "Skills"),
        };
    }
}
