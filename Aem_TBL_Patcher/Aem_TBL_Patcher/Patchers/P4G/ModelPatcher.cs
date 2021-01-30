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
            new Segment(0, "ModelUnknown1"),
            new Segment(0, "ModelUnknown2")
        };
    }
}
