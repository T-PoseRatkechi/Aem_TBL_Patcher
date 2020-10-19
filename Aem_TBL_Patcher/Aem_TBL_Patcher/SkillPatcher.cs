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
            GetElementPatches(thePatches, originalBytes, moddedBytes);
            GetSkillPatches(thePatches, originalBytes, moddedBytes);
            Console.WriteLine($"SKL - Total Patches Needed: {thePatches.Count}");

            return thePatches;
        }

        private void GetElementPatches(List<PatchEdit> patchesList, byte[] originalBytes, byte[] moddedBytes)
        {
            int startOffset = 0;
            UInt32 chunkSize = BitConverter.ToUInt32(moddedBytes[0..4]);

            int elementSize = 2;

            int elementsParsed = 0;
            int patchesCount = 0;

            for (int currentByte = startOffset + 4, totalBytes = (int)(chunkSize + 4); currentByte < totalBytes; currentByte+=elementSize)
            {
                elementsParsed++;
                byte[] originalElement = originalBytes[currentByte..(currentByte + elementSize)];
                byte[] moddedElement = moddedBytes[currentByte..(currentByte + elementSize)];

                if (!originalElement.SequenceEqual(moddedElement))
                {
                    patchesList.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedElement });
                    patchesCount++;
                }
            }

            Console.WriteLine($"SKL - Elements Parsed: {elementsParsed}, Total Patches: {patchesCount}");
        }

        private void GetSkillPatches(List<PatchEdit> patchesList, byte[] originalBytes, byte[] moddedBytes)
        {
            int startOffset = 1264;

            UInt32 chunkSize = BitConverter.ToUInt32(moddedBytes[startOffset..(startOffset + 4)]);
            int skillSize = 44;

            int skillsParsed = 0;
            int patchesCount = 0;

            for (int currentByte = startOffset + 4, totalBytes = (int)(startOffset + 4 + chunkSize); currentByte < totalBytes; currentByte += skillSize)
            {
                skillsParsed++;
                byte[] originalElement = originalBytes[currentByte..(currentByte + skillSize)];
                byte[] moddedElement = moddedBytes[currentByte..(currentByte + skillSize)];

                if (!originalElement.SequenceEqual(moddedElement))
                {
                    patchesList.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedElement });
                    patchesCount++;
                }
            }

            Console.WriteLine($"SKL - Skills Parsed: {skillsParsed}, Total Patches: {patchesCount}");
        }
    }
}
