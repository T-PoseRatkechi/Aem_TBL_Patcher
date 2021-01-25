using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class MsgPatcher : BasePatcher
    {
        public MsgPatcher() : base() { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[]
        {
            new ListPatches(0, 21, "Arcana Names"),
            new ListPatches(688, 23, "Skill Names"),
            new ListPatches(15056, 21, "Enemy Names"),
            new ListPatches(22800, 21, "Persona Names"),
        };
    }
}
