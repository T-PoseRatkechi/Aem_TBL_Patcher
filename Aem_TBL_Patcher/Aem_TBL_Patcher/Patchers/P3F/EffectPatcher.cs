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
            new Segment(0, "EFFECTUnknown1"),
            new Segment(0, "EFFECTUnknown2"),
            new Segment(0, "EFFECTUnknown3"),
            new Segment(0, "EFFECTUnknown4"),
            new Segment(0, "EFFECTUnknown5"),
            new Segment(0, "EFFECTUnknown6")
        };
    }
}
