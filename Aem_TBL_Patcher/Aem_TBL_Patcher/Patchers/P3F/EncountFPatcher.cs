using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class EncountFPatcher : AutoBasePatcher
    {
        public EncountFPatcher() : base("ENCOUNT_F", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(28, "Encounter"),
            new Segment(0, "EncounterUnknown1"),
            new Segment(0, "EncounterUnknown2"),
            new Segment(0, "EncounterUnknown3"),
            new Segment(0, "EncounterUnknown4")
        };
    }
}