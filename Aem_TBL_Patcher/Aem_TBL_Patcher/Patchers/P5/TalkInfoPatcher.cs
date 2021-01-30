using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class TalkInfoPatcher : AutoBasePatcher
    {
        public TalkInfoPatcher() : base("TALKINFO", true) { }

        //protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new BytePatches(_tblName, 0, _originalBytes.Length) };
        protected override Segment[] Segments => new Segment[]
        {
            new Segment(0, "TalkInfoUnknown")
        };
    }
}
