using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class BytePatches : IPatchGenerator
    {
        string tbl = null;
        int patchByteStart = 0;
        int patchByteEnd = 0;

        public BytePatches(string tblName, int startByte, int endByte)
        {
            tbl = tblName;
            patchByteStart = startByte;
            patchByteEnd = endByte;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            // handle creating patches upto length of originaBytes array
            for (int byteIndex = patchByteStart; byteIndex < patchByteEnd; byteIndex++)
            {
                byte currentOriginalByte = originalBytes[byteIndex];
                byte currentModdedByte = moddedBytes[byteIndex];

                // mismatched bytes indicating edited bytes
                if (currentOriginalByte != currentModdedByte)
                {
                    PatchEdit newPatch = new PatchEdit
                    {
                        tbl = tbl,
                        offset = byteIndex
                    };

                    // read ahead for the edited bytes
                    for (int byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < patchByteEnd; byteEditIndex++, byteCount++)
                    {
                        // handle creating patches that reach up to the length of the original tbl
                        if (byteEditIndex == originalBytes.Length - 1)
                        {
                            byte[] tempData = new byte[byteCount + 1];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount + 1);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex;
                            break;
                        }
                        // handle creating patches within the range of the original tbl
                        else if (originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                        {
                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex;
                            break;
                        }
                    }

                    patches.Add(newPatch);
                }
            }

            // handle creating patches past the original length of the tbl
            // if the modded tbl extends past that and a patch was wanted for the entire tbl
            if (patchByteEnd >= originalBytes.Length && originalBytes.Length < moddedBytes.Length)
            {
                byte[] extendedBytes = moddedBytes[originalBytes.Length..moddedBytes.Length];

                PatchEdit newPatch = new PatchEdit
                {
                    tbl = tbl,
                    offset = originalBytes.Length,
                    data = PatchDataFormatter.ByteArrayToHexText(extendedBytes)
                };

                Console.WriteLine("Extended Byte Patch");

                patches.Add(newPatch);
            }
        }
    }
}
