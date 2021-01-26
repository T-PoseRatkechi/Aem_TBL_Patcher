using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class UnitPatcher : AutoBasePatcher
    {
        public UnitPatcher() : base("UNIT", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(68, "Enemy Unit Stats"),
            new Segment(40, "Enemy Affinities"),
            new Segment(40, "Persona Affinities"),
            new Segment(24, "Enemy Visual Properties"),
            new Segment(4, "Enemy Visual TBL Index")
        };
    }
}