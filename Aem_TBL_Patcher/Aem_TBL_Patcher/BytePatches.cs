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
            // track patch info
            int totalBytesEdited = 0;
            int patchesCount = 0;
            int extraPatchesCount = 0;

            for (int byteIndex = 0; byteIndex < _segmentProps.OriginalSize && byteIndex < _segmentProps.ModSize; byteIndex++)
            {
                int original_byteIndex = _segmentProps.OriginalOffset + byteIndex;
                byte currentOriginalByte = originalBytes[original_byteIndex];

                int modded_byteIndex = _segmentProps.ModOffset + byteIndex;
                byte currentModdedByte = moddedBytes[modded_byteIndex];

                // mismatched bytes indicating edited bytes
                if (currentOriginalByte != currentModdedByte)
                {
                    PatchEdit newPatch = new PatchEdit
                    {
                        tbl = _segmentProps.Tbl,
                        section = _segmentProps.Index,
                        offset = byteIndex
                    };

                    // read ahead for the edited bytes
                    for (int byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < _segmentProps.OriginalSize && byteEditIndex < _segmentProps.ModSize; byteEditIndex++, byteCount++)
                    {
                        // handle creating patches that reach up to the length of the original tbl
                        if (byteEditIndex >= _segmentProps.OriginalSize - 1)
                        {
                            totalBytesEdited += byteCount + 1;

                            byte[] tempData = new byte[byteCount + 1];
                            Array.Copy(moddedBytes, modded_byteIndex, tempData, 0, byteCount + 1);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex;
                            break;
                        }
                        // handle creating patches within the range of the original tbl
                        else if (originalBytes[_segmentProps.OriginalOffset + byteEditIndex] == moddedBytes[_segmentProps.ModOffset + byteEditIndex])
                        {
                            totalBytesEdited += byteCount;

                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, modded_byteIndex, tempData, 0, byteCount);
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
                    tbl = _segmentProps.Tbl,
                    section = _segmentProps.Index,
                    offset = (int)_segmentProps.OriginalSize,
                    data = PatchDataFormatter.ByteArrayToHexText(tempData)
                };

                extraPatchesCount++;
                patches.Add(newPatch);
            }
            else if (_segmentProps.ModSize < _segmentProps.OriginalSize)
            {
                PatchEdit newPatch = new PatchEdit
                {
                    tbl = _segmentProps.Tbl,
                    section = _segmentProps.Index,
                    offset = (int) _segmentProps.ModSize,
                    data = PatchDataFormatter.ByteArrayToHexText(new byte[_segmentProps.OriginalSize - _segmentProps.ModSize])
                };

                Console.WriteLine($"[{_segmentProps.Name}] Modded segment is smaller than original segment! Adding null bytes upto original length of segment!");
                patches.Add(newPatch);
            }

            Console.WriteLine($"[{_segmentProps.Name}] Bytes Edited: {totalBytesEdited}, Total Patches: {patchesCount + extraPatchesCount}, Extended Patches: {extraPatchesCount}");
        }
    }
}
