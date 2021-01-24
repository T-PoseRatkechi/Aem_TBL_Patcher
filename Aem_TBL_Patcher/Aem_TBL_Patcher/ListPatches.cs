using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    class ListPatches : IPatchGenerator
    {
        int startByte = 0;
        int size = 0;
        string name = null;

        public ListPatches(int startOffset, int itemSize, string elementName)
        {
            startByte = startOffset;
            size = itemSize;
            name = elementName;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            byte[] chunkSizeBytes = moddedBytes[startByte..(startByte + 4)];

            if (chunkSizeBytes[3] != 0x00)
            {
                Console.WriteLine("Using BE");
                Array.Reverse(chunkSizeBytes);
            }
            else
                Console.WriteLine("Using LE");

            UInt32 chunkSize = BitConverter.ToUInt32(chunkSizeBytes);

            int endByte = (int)(startByte + 4 + chunkSize);

            int elementsParsed = 0;
            int patchesCount = 0;

            for (int currentByte = startByte + 4; currentByte < endByte; currentByte += size)
            {
                byte[] originalElement = originalBytes[currentByte..(currentByte + size)];
                byte[] moddedElement = moddedBytes[currentByte..(currentByte + size)];

                if (!originalElement.SequenceEqual(moddedElement))
                {
                    patches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedElement });
                    patchesCount++;
                }

                elementsParsed++;
            }

            Console.WriteLine($"{name} Parsed: {elementsParsed}, Total Patches: {patchesCount}");
        }
    }
}
