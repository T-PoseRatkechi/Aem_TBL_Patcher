using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class UnitPatcher : BasePatcher
    {
        public UnitPatcher(byte[] originalBytes, byte[] moddedBytes) : base() { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] 
        {
            new ListPatches(0, 60, "Enemy Unit Stats"),
            new ListPatches(22096, 32, "Enemy Affinities"),
            new ListPatches(33888, 32, "Persona Affinities"),
        };
    }
}
