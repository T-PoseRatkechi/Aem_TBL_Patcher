using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class PersonaPatcher : AutoBasePatcher
    {
        public PersonaPatcher() : base("PERSONA", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(14, "Persona Stats"),
            new Segment(70, "Persona Growths"),
            new Segment(392, "Party LV UP Thresholds"),
            new Segment(622, "Party Personas")
        };
    }
}