using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class PersonaPatcher : BasePatcher
    {
        public PersonaPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[]
        {
            new ListPatches(0, 14, "Persona Stats"),
            new ListPatches(3600, 70, "Persona Growths"),
            new ListPatches(21536, 622, "Party Personas"),
            new ListPatches(36480, 392, "Party LV UP Thresholds"),
            new ListPatches(40016, 2, "Persona Exist"),
            new ListPatches(40464, 4, "Persona Fusion"),
        };
    }
}
