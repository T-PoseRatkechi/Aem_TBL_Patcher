using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class TalkInfoPatcher : BasePatcher
    {
        public TalkInfoPatcher() : base("TALKINFO", true) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new BytePatches(0, _moddedBytes.Length) };
    }
}
