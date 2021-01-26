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

        readonly public string _tblName = null;
        readonly protected bool _isBigEndian = false;

        public BasePatcher(string tblName, bool isBigEndian)
        {
            _tblName = tblName;
            _isBigEndian = isBigEndian;
        }

        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            // set tbl byte arrays
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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Patches Created: {_thePatches.Count}");
            Console.ResetColor();
        }

        protected abstract IPatchGenerator[] Patchers { get; }
    }
}
