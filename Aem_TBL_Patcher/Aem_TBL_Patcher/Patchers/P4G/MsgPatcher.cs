using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class MsgPatcher : AutoBasePatcher
    {
        public MsgPatcher() : base("MSG", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(21, "Arcana Names"),
            new Segment(23, "Skill Names"),
            new Segment(21, "Enemy Names"),
            new Segment(21, "Persona Names"),
        };
    }
}
