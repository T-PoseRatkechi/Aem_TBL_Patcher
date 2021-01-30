using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P3F
{
    class EffectPatcher : AutoBasePatcher
    {
        public EffectPatcher() : base("EFFECT", false) { }

        protected override Segment[] Segments => new Segment[]
{
            new Segment("EFFECTUnknown1"),
            new Segment("EFFECTUnknown2"),
            new Segment("EFFECTUnknown3"),
            new Segment("EFFECTUnknown4"),
            new Segment("EFFECTUnknown5"),
            new Segment("EFFECTUnknown6")
        };
    }
}
