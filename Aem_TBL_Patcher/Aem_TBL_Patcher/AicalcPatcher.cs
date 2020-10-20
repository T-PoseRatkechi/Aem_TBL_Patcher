using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class AicalcPatcher : BasePatcher
    {
        public AicalcPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new BytePatches(0, _moddedBytes.Length) };
    }
}
