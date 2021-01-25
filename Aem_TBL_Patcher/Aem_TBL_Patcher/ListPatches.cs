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
        bool isBE = false;

        public ListPatches(int startOffset, int itemSize, string elementName)
        {
            startByte = startOffset;
            size = itemSize;
            name = elementName;
        }

        public ListPatches(int startOffset, int itemSize, string elementName, bool be)
        {
            startByte = startOffset;
            size = itemSize;
            name = elementName;
            isBE = be;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            byte[] chunkSizeBytes = moddedBytes[startByte..(startByte + 4)];

            if (isBE)
                Array.Reverse(chunkSizeBytes);

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
                    patches.Add(new PatchEdit() { offset = currentByte, data = ByteArrayToString(moddedElement) });
                    patchesCount++;
                }

                elementsParsed++;
            }

            Console.WriteLine($"{name} Parsed: {elementsParsed}, Total Patches: {patchesCount}");
        }
        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }

    }
}
