using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class BytePatches : IPatchGenerator
    {
        string tbl = null;
        int start = 0;
        int end = 0;

        public BytePatches(string tblName, int startByte, int endByte)
        {
            tbl = tblName;
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
                        tbl = tbl,
                        offset = byteIndex
                    };

                    // read ahead for the edited bytes
                    for (int byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < totalBytes; byteEditIndex++, byteCount++)
                    {
                        // exit loop once bytes match again
                        if (byteEditIndex == totalBytes - 1 || originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                        {
                            if (byteEditIndex == totalBytes - 1)
                                byteCount += 1;
                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            if (byteEditIndex == totalBytes - 1)
                                byteIndex = totalBytes;
                            else
                                byteIndex = byteEditIndex - 1;
                            break;
                        }
                    }

                    /* cooler null implementation
                    if (newPatch.data == null)
                    {
                        byte[] tempData = new byte[totalBytes - byteIndex];
                        Array.Copy(moddedBytes, byteIndex, tempData, 0, totalBytes - byteIndex);
                        newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                        byteIndex = totalBytes;
                    }
                    */
                    patches.Add(newPatch);
                }
            }
            // add rest of expanded bytes as patch
            if (moddedBytes.Length > originalBytes.Length)
            {
                PatchEdit newPatch = new PatchEdit
                {
                    tbl = tbl,
                    offset = originalBytes.Length,
                    data = PatchDataFormatter.ByteArrayToHexText(moddedBytes[originalBytes.Length..moddedBytes.Length])
                };
                patches.Add(newPatch);
            }
        }
    }
}
