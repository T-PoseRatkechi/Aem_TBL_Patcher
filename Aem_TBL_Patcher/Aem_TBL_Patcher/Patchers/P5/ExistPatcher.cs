using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class ExistPatcher : AutoBasePatcher
    {
        public ExistPatcher() : base("EXIST", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(0, "ExistUnknown")
        };
    }
}
