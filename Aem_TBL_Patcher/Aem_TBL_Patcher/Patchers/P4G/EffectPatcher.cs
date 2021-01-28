using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class EffectPatcher : BasePatcher
    {
        public EffectPatcher() : base("EFFECT", false) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new BytePatches(_tblName, 0, _originalBytes.Length) };
    }
}
