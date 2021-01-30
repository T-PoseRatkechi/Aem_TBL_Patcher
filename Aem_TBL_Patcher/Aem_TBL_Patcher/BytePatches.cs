﻿using System;
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
                        // exit loop once bytes match again
                        if (originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                        {
                            byte[] tempData = new byte[byteCount];
                            Array.Copy(moddedBytes, byteIndex, tempData, 0, byteCount);
                            newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                            byteIndex = byteEditIndex - 1;
                            break;
                        }
                    }

                    if (newPatch.data == null)
                    {
                        byte[] tempData = new byte[patchBytesEnd - byteIndex];
                        Array.Copy(moddedBytes, byteIndex, tempData, 0, patchBytesEnd - byteIndex);
                        newPatch.data = PatchDataFormatter.ByteArrayToHexText(tempData);
                        byteIndex = patchBytesEnd;
                    }

                    patches.Add(newPatch);
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
                patches.Add(newPatch);
            }
        }
    }
}
