using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher
{
    class BytePatches : IPatchGenerator
    {
        readonly SegmentProps _segmentProps;

        public BytePatches(SegmentProps segmentProps)
        {
            _segmentProps = segmentProps;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            string tbl = _segmentProps.Tbl;
            int patchBytesStart = _segmentProps.OriginalOffset;
            int patchBytesEnd = (int)_segmentProps.OriginalSize + patchBytesStart;

            // track patch info
            int totalBytesEdited = 0;
            int patchesCount = 0;
            int extraPatchesCount = 0;

            for (int byteIndex = patchBytesStart; byteIndex < patchBytesEnd; byteIndex++)
            {
                byte currentOriginalByte = originalBytes[byteIndex];
                byte currentModdedByte = moddedBytes[byteIndex];

                // mismatched bytes indicating edited bytes
                if (currentOriginalByte != currentModdedByte)
                {
                    PatchEdit newPatch = new PatchEdit
                    {
                        tbl = tbl,
                        section = _segmentProps.Index,
                        offset = byteIndex - 4
                    };

                    // read ahead for the edited bytes
                    for (int byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < patchBytesEnd; byteEditIndex++, byteCount++)
                    {
                        // handle creating patches that reach up to the length of the original tbl
                        if (byteEditIndex == originalBytes.Length - 1)
                        {
                            totalBytesEdited += byteCount + 1;

                            byte[] tempData = new byte[byteCount + 1];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount + 1);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex;
                            break;
                        }
                        // handle creating patches within the range of the original tbl
                        else if (originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                        {
                            totalBytesEdited += byteCount;

                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex;
                            break;
                        }
                    }
                    
                    patches.Add(newPatch);
                    patchesCount++;
                }
            }
            // add rest of expanded bytes as patch
            if (_segmentProps.ModSize > _segmentProps.OriginalSize)
            {
                int expandedSize = (int)(_segmentProps.ModSize - _segmentProps.OriginalSize);

                byte[] tempData = new byte[expandedSize];
                Array.Copy(moddedBytes, (int)(_segmentProps.OriginalSize + _segmentProps.ModOffset), tempData, 0, expandedSize);

                PatchEdit newPatch = new PatchEdit
                {
                    tbl = tbl,
                    section = _segmentProps.Index,
                    offset = (int)_segmentProps.OriginalSize,
                    data = PatchDataFormatter.ByteArrayToHexText(tempData)
                };

                extraPatchesCount++;
                patches.Add(newPatch);
            }

            Console.WriteLine($"[{_segmentProps.Name}] Bytes Edited: {totalBytesEdited}, Total Patches: {patchesCount + extraPatchesCount}, Extended Patches: {extraPatchesCount}");
        }
    }
}
