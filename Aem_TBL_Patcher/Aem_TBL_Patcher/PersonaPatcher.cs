using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class PersonaPatcher : IPatcher
    {
        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            ListPatcher[] patchers = new ListPatcher[] 
            {
                new ListPatcher(0, 14, "Persona Stats"),
                new ListPatcher(3600, 70, "Persona Growths"),
                new ListPatcher(21536, 622, "Party Personas"),
                new ListPatcher(36480, 392, "Party LV UP Thresholds"),
                new ListPatcher(40016, 2, "Persona Exist"),
                new ListPatcher(40464, 4, "Persona Fusion")
            };

            foreach (ListPatcher patcher in patchers)
            {
                patcher.GenerateListPatches(thePatches, originalBytes, moddedBytes);
            }

            Console.WriteLine($"Total Patches: {thePatches.Count}");

            return thePatches;
        }
    }
}
