using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class ModelPatcher : AutoBasePatcher
    {
        public ModelPatcher() : base("MODEL", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment("ModelUnknown1"),
            new Segment("ModelUnknown2"),
            new Segment("ModelUnknown3"),
        };
    }
}
