using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class AicalcPatcher : AutoBasePatcher
    {
        public AicalcPatcher() : base("AICALC", false) { }

        // TODO
        protected override Segment[] Segments => throw new NotImplementedException();
    }
}