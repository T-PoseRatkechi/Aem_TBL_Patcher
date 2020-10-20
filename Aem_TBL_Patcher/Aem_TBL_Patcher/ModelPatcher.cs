using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class ModelPatcher : IPatcher
    {
        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            BytePatcher patcher = new BytePatcher();
            patcher.GenerateBytePatches(thePatches, originalBytes, moddedBytes);

            return thePatches;
        }
    }
}
