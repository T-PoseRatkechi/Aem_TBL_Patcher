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

            GetUnitStatPatches(thePatches, originalBytes, moddedBytes);
            GetUnitAffinityPatches(thePatches, originalBytes, moddedBytes);
            GetPersonaAffinityPatches(thePatches, originalBytes, moddedBytes);

            Console.WriteLine($"Total Patches: {thePatches.Count}");

            return thePatches;
        }

        private void GetUnitStatPatches(List<PatchEdit> patchesList, byte[] originalBytes, byte[] moddedBytes)
        {
            ListPatcher patcher = new ListPatcher(0, 60, "Enemy Unit Stats");
            patcher.GenerateListPatches(patchesList, originalBytes, moddedBytes);
        }

        private void GetUnitAffinityPatches(List<PatchEdit> patchesList, byte[] originalBytes, byte[] moddedBytes)
        {
            ListPatcher patcher = new ListPatcher(22096, 32, "Enemy Affinities");
            patcher.GenerateListPatches(patchesList, originalBytes, moddedBytes);
        }

        private void GetPersonaAffinityPatches(List<PatchEdit> patchesList, byte[] originalBytes, byte[] moddedBytes)
        {
            ListPatcher patcher = new ListPatcher(33888, 32, "Persona Affinities");
            patcher.GenerateListPatches(patchesList, originalBytes, moddedBytes);
        }
    }
}
