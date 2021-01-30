using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class TalkInfoPatcher : AutoBasePatcher
    {
        public TalkInfoPatcher() : base("TALKINFO", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(0, "TalkInfoUnknown")
        };
    }
}
