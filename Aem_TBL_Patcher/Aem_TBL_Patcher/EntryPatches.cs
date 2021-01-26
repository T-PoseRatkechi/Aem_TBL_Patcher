using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher
{
    class EntryPatches : IPatchGenerator
    {
        readonly SegmentProps _segmentProps;

        public EntryPatches(SegmentProps segmentProps)
        {
            _segmentProps = segmentProps;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            int patchesCount = 0;

            int original_numEntries = (int)(_segmentProps.OriginalSize / _segmentProps.EntrySize);
            int mod_numEntries = (int)(_segmentProps.ModSize / _segmentProps.EntrySize);

            for (int currentEntry = 0; currentEntry < mod_numEntries; currentEntry++)
            {
                int original_currentOffset = _segmentProps.OriginalOffset + (currentEntry * _segmentProps.EntrySize);
                int mod_currentOffset = _segmentProps.ModOffset + (currentEntry * _segmentProps.EntrySize);

                if (currentEntry < original_numEntries)
                {
                    byte[] originalElement = originalBytes[original_currentOffset..(original_currentOffset + _segmentProps.EntrySize)];
                    byte[] moddedElement = moddedBytes[mod_currentOffset..(mod_currentOffset + _segmentProps.EntrySize)];

                    if (!originalElement.SequenceEqual(moddedElement))
                    {
                        patches.Add(new PatchEdit
                        {
                            comment = $"Segment: {_segmentProps.Name}, Index: {currentEntry}",
                            tbl = _segmentProps.Tbl,
                            section = _segmentProps.Index,
                            offset = (currentEntry * _segmentProps.EntrySize),
                            data = ByteArrayToString(moddedElement),
                        });

                        //Console.WriteLine($"Entry Patch\nTBL: {_segmentProps.Tbl}\nSegmentIndex: {_segmentProps.Index}\nOffset: {currentEntry * _segmentProps.EntrySize}\n");

                        patchesCount++;
                    }
                }
                else
                {
                    byte[] moddedElement = moddedBytes[mod_currentOffset..(mod_currentOffset + _segmentProps.EntrySize)];
                    
                    patches.Add(new PatchEdit
                    {
                        comment = $"Segment: {_segmentProps.Name} (Extended), Index: {currentEntry}",
                        tbl = _segmentProps.Tbl,
                        section = _segmentProps.Index,
                        offset = (currentEntry * _segmentProps.EntrySize),
                        data = ByteArrayToString(moddedElement),
                    });

                    //Console.WriteLine($"Entry Patch (Extended)\nTBL: {_segmentProps.Tbl}\nSegmentIndex: {_segmentProps.Index}\nOffset: {currentEntry * _segmentProps.EntrySize}\n");

                    patchesCount++;
                }
            }

            //Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{_segmentProps.Name}]\nEntries: {mod_numEntries}, Patches: {patchesCount}");
            //Console.ResetColor();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }
    }
}
