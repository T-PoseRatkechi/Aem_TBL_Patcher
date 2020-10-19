using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    class UnitPatcher : IPatcher
    {
        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            ListPatcher[] patchers = new ListPatcher[] {
                new ListPatcher(0, 60, "Enemy Unit Stats"),
                new ListPatcher(22096, 32, "Enemy Affinities"),
                new ListPatcher(33888, 32, "Persona Affinities"),
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
