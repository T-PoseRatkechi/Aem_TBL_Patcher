using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    abstract class BasePatcher
    {
        protected List<PatchEdit> _thePatches = new List<PatchEdit>();
        protected byte[] _originalBytes;
        protected byte[] _moddedBytes;

        public BasePatcher()
        {

        }

        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            _originalBytes = originalBytes;
            _moddedBytes = moddedBytes;

            LoadPatches();
            return _thePatches;
        }

        protected virtual void LoadPatches()
        {
            foreach (IPatchGenerator patcher in Patchers)
            {
                patcher.GeneratePatches(_thePatches, _originalBytes, _moddedBytes);
            }

            Console.WriteLine($"Total Patches: {_thePatches.Count}");
        }

        protected abstract IPatchGenerator[] Patchers { get; }
    }
}
