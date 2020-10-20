using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    interface IPatchGenerator
    {
        void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes);
    }
}
