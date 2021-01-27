using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class BytePatches : IPatchGenerator
    {
        int start = 0;
        int end = 0;

        public BytePatches(int startByte, int endByte)
        {
            start = startByte;
            end = endByte;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            for (int byteIndex = start, totalBytes = end; byteIndex < totalBytes; byteIndex++)
            {
                byte currentOriginalByte = originalBytes[byteIndex];
                byte currentModdedByte = moddedBytes[byteIndex];

                // mismatched bytes indicating edited bytes
                if (currentOriginalByte != currentModdedByte)
                {
                    PatchEdit newPatch = new PatchEdit
                    {
                        offset = byteIndex
                    };

                    // read ahead for the edited bytes
                    for (int byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < totalBytes; byteEditIndex++, byteCount++)
                    {
                        // exit loop once bytes match again
                        if (originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                        {
                            //newPatch.data = new byte[byteCount];
                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount);
                            byteIndex = byteEditIndex - 1;
                            break;
                        }
                    }

                    patches.Add(newPatch);
                }
            }
        }
    }
}
