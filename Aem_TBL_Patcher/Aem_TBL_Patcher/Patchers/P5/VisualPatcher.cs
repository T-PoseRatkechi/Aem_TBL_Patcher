using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class VisualPatcher : AutoBasePatcher
    {
        public VisualPatcher() : base("VISUAL", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(200, "Enemy Visual Data A"),
            new Segment(404, "Player Visual Data A"),
            new Segment(148, "Persona Visual Data A"),
            new Segment(648, "Player Visual Data B"),
            new Segment(360, "Enemy Visual Data B"),
            new Segment(576, "Persona Visual Data B")
        };
    }
}