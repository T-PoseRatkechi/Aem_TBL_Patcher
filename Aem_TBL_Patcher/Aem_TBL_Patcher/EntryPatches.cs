using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    class EntryPatches : IPatchGenerator
    {
        string tblName = String.Empty;
        string sectionName = String.Empty;
        int sectionIndex = 0;
        int segmentOffset = 0;
        uint segmentSize = 0;
        int entrySize = 0;

        public EntryPatches(string tbl, string name, int index, int offset, uint size, int itemSize)
        {
            tblName = tbl;
            sectionName = name;
            sectionIndex = index;
            segmentOffset = offset;
            segmentSize = size;
            entrySize = itemSize;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            int patchesCount = 0;

            for (int currentEntry = 0, numEntries = (int)(segmentSize / entrySize); currentEntry < numEntries; currentEntry++)
            {
                int currentOffset = segmentOffset + (currentEntry * entrySize);

                byte[] originalElement = originalBytes[currentOffset..(currentOffset + entrySize)];
                byte[] moddedElement = moddedBytes[currentOffset..(currentOffset + entrySize)];

                if (!originalElement.SequenceEqual(moddedElement))
                {
                    patches.Add(new PatchEdit() {
                        comment = $"Segment: {sectionName}, Entry Index: {currentEntry}",
                        tbl = tblName,
                        section = sectionIndex,
                        offset = (currentEntry * entrySize),
                        data = moddedElement
                    });

                    Console.WriteLine($"Entry Patch\nTBL: {tblName}\nSection: {sectionIndex}\nOffset: {currentEntry * entrySize}\nData: {ByteArrayToString(moddedElement)}");

                    patchesCount++;
                }
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }
    }
}
