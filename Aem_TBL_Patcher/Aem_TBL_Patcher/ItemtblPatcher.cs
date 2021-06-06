using System;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher
{
    abstract class ItemtblPatcher : BasePatcher
    {
        public ItemtblPatcher(string tblName, bool isBigEndian) : base(tblName, isBigEndian) { }

        private IPatchGenerator[] GeneratePatchers()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{_tblName}");
            Console.ResetColor();

            int totalSegments = Segments.Length;

            // create array of patchers
            IPatchGenerator[] patchers = new IPatchGenerator[totalSegments];
            ushort original_SegmentEntryCount = 0;
            ushort mod_SegmentEntryCount = 0;
            // generate a ListPatcher for each segment
            for (int segmentIndex = 0, original_currentOffset = 0, mod_currentOffset = 0; segmentIndex < totalSegments; segmentIndex++)
            {
                Segment currentSegment = Segments[segmentIndex];

                if (mod_SegmentEntryCount == 0 && original_SegmentEntryCount == 0)
                {
                    // store segment size bytes of original and modded
                    byte[] original_SegmentEntryCountBytes = _originalBytes[original_currentOffset..(original_currentOffset + 2)];
                    original_currentOffset += 2;
                    byte[] mod_SegmentEntryCountBytes = _moddedBytes[mod_currentOffset..(mod_currentOffset + 2)];
                    mod_currentOffset += 2;

                    // reverse bytes if big endian
                    if (_isBigEndian)
                    {
                        Array.Reverse(original_SegmentEntryCountBytes);
                        Array.Reverse(mod_SegmentEntryCountBytes);
                    }

                    original_SegmentEntryCount = BitConverter.ToUInt16(original_SegmentEntryCountBytes);
                    mod_SegmentEntryCount = BitConverter.ToUInt16(mod_SegmentEntryCountBytes);
                }

                var original_segmentSize = original_SegmentEntryCount * (uint)currentSegment.EntrySize;
                var mod_segmentSize = mod_SegmentEntryCount * (uint)currentSegment.EntrySize;

                // if segment uses entries, use EntryPatches
                if (currentSegment.UseEntries)
                {
                    patchers[segmentIndex] = new ItemtblEntryPatches(new SegmentProps
                    {
                        Tbl = _tblName,
                        Index = 0, // Aemulus currently treats it as a single section
                        Name = currentSegment.SegmentName,
                        EntrySize = currentSegment.EntrySize,
                        OriginalOffset = original_currentOffset,
                        ModOffset = mod_currentOffset,
                        OriginalSize = original_segmentSize,
                        ModSize = mod_segmentSize,
                    });
                }

                // update current offset
                original_currentOffset = (int)(original_currentOffset + original_segmentSize);
                mod_currentOffset = (int)(mod_currentOffset + mod_segmentSize);

                // Add 30 unknown bytes (seems to be unused overflow from item data)
                original_currentOffset += 30;
                mod_currentOffset += 30;
            }

            return patchers;
        }

        protected override IPatchGenerator[] Patchers => GeneratePatchers();

        protected abstract Segment[] Segments { get; }
    }
}
