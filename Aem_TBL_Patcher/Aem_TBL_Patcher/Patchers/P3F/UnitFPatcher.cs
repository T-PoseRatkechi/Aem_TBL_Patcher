using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class UnitFPatcher : AutoBasePatcher
    {
        public UnitFPatcher() : base("UNIT_F", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(62, "Enemy Unit Stats"),
            new Segment(32, "Enemy Elemental Affinities"),
            new Segment(32, "Persona Elemental Affinities"),
        };
    }
}
