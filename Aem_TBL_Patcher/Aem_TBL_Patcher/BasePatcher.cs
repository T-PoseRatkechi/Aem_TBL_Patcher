using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    abstract class BasePatcher
    {
        protected byte[] _originalBytes;
        protected byte[] _moddedBytes;

        public BasePatcher(byte[] originalBytes, byte[] moddedBytes)
        {
            _originalBytes = originalBytes;
            _moddedBytes = moddedBytes;
        }

        public virtual List<PatchEdit> GetPatches()
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            foreach (IPatchGenerator patcher in Patchers)
            {
                patcher.GeneratePatches(thePatches, _originalBytes, _moddedBytes);
            }

            Console.WriteLine($"Total Patches: {thePatches.Count}");

            return thePatches;
        }

        protected abstract IPatchGenerator[] Patchers { get; }
    }
}
