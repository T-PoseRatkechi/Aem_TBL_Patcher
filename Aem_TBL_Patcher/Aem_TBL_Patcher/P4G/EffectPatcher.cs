using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class EffectPatcher : BasePatcher
    {
        public EffectPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new BytePatches(0, _moddedBytes.Length) };
    }
}
