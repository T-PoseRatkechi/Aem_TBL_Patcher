using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class NamePatchGenerator : IPatchGenerator
    {
        List<List<byte[]>> GetNameSections(byte[] tblBytes)
        {
            List<byte[]> section;
            List<List<byte[]>> sections = new List<List<byte[]>>();
            int pos = 0;

            // 17 sections
            for (int i = 0; i <= 16; i++)
            {
                section = new List<byte[]>();
                // Get big endian section size
                byte[] pointersSizeBytes = tblBytes[pos..(pos + 4)];
                Array.Reverse(pointersSizeBytes);
                int pointersSize = BitConverter.ToInt32(pointersSizeBytes, 0);

                // Get to name section
                pos += pointersSize + 4;
                if ((pos % 16) != 0)
                {
                    pos += 16 - (pos % 16);
                }

                // Get big endian section size
                byte[] namesSizeBytes = tblBytes[pos..(pos + 4)];
                Array.Reverse(namesSizeBytes);
                int namesSize = BitConverter.ToInt32(namesSizeBytes, 0);

                // Get names
                byte[] segment = tblBytes[(pos + 4)..(pos + 4 + namesSize)];
                List<byte> name = new List<byte>();

                foreach (var segmentByte in segment)
                {
                    if (segmentByte == (byte)0)
                    {
                        section.Add(name.ToArray());
                        name = new List<byte>();
                    }
                    else
                    {
                        name.Add(segmentByte);
                    }
                }

                // Get to next section
                pos += namesSize + 4;

                if ((pos % 16) != 0)
                {
                    pos += 16 - (pos % 16);
                }

                sections.Add(section);
            }
            return sections;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("NAME");
            Console.ResetColor();

            List<List<byte[]>> originalNameSections = GetNameSections(originalBytes);
            List<List<byte[]>> moddedNameSections = GetNameSections(moddedBytes);

            if (originalNameSections.Count != 17 && moddedNameSections.Count != 17)
            {
                Console.WriteLine("Incorrect number of sections for NAME.TBL!");
            }

            for (int i = 0; i <= 16; i++)
            {
                int numNames = Math.Max(originalNameSections[i].Count, moddedNameSections[i].Count);
                for (int j = 0; j < numNames; j++)
                {
                    if (originalNameSections[i].Count <= j || !originalNameSections[i][j].SequenceEqual(moddedNameSections[i][j]))
                    {
                        patches.Add(new PatchEdit
                        {
                            comment = $"Segment: {i}, Index: {j}",
                            tbl = "NAME",
                            section = i,
                            index = j,
                            name = PatchDataFormatter.ByteArrayToNameText(moddedNameSections[i][j]),
                        });
                    }
                }
            }
        }
    }

    class NamePatcher : BasePatcher
    {
        public NamePatcher() : base("NAME", true) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[] { new NamePatchGenerator() };
    }
}