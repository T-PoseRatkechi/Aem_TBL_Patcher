using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class ModelPatcher : AutoBasePatcher
    {
        public ModelPatcher() : base("MODEL", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(382, "Player Visual Variables"),
            new Segment(232, "Enemy Visual Variables"),
            new Segment(88, "Persona Visual Variables")
        };
    }
}
