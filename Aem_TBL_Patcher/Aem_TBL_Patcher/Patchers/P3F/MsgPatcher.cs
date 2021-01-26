using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class MsgPatcher : AutoBasePatcher
    {
        public MsgPatcher() : base("MSG", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(21, "Arcana Names"),
            new Segment(19, "Skill Names"),
            new Segment(19, "Enemy Names"),
            new Segment(17, "Persona Names")
        };
    }
}
