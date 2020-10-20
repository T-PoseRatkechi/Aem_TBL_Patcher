using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class MsgPatcher : IPatcher
    {
        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            ListPatcher[] patchers = new ListPatcher[]
            {
                new ListPatcher(0, 21, "Arcana Names"),
                new ListPatcher(688, 23, "Skill Names"),
                new ListPatcher(15056, 21, "Enemy Names"),
                new ListPatcher(22800, 21, "Persona Names"),
                new ListPatcher(28192, 1, "Unknown 1"), // Maybe wasteful? Prefer the old method?
            };

            foreach (ListPatcher patcher in patchers)
            {
                patcher.GenerateListPatches(thePatches, originalBytes, moddedBytes);
            }

            return thePatches;
        }
    }
}
