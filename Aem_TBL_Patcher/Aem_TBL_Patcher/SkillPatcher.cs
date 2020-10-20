using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    class SkillPatcher : IPatcher
    {
        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();
            //GetElementPatches(thePatches, originalBytes, moddedBytes);
            //GetSkillPatches(thePatches, originalBytes, moddedBytes);
            ListPatcher[] patchers = new ListPatcher[]
            {
                new ListPatcher(0, 2, "Elements"),
                new ListPatcher(1264, 44, "Skills"),
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
