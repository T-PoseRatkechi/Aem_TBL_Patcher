using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class SkillPatcher : BasePatcher
    {
        public SkillPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] {
            new ListPatches(0, 2, "Elements"),
            new ListPatches(1264, 44, "Skills"),
        };
    }
}
