using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class UnitPatcher : AutoBasePatcher
    {
        public UnitPatcher() : base("UNIT", false) { }

        protected override Segment[] Segments => new Segment[] 
        {
            new Segment(60, "Enemy Unit Stats"),
            new Segment(32, "Enemy Affinities"),
            new Segment(32, "Persona Affinities"),
        };
    }
}
