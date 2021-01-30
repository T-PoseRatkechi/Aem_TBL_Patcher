using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class EffectPatcher : AutoBasePatcher
    {
        public EffectPatcher() : base("EFFECT", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment("EffectUnknown")
        };
    }
}
