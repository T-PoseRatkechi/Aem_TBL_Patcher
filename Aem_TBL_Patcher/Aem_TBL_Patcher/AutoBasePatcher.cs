using System;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher
{
    abstract class AutoBasePatcher : BasePatcher
    {
        public AutoBasePatcher(string tblName, bool isBigEndian) : base(tblName, isBigEndian) { }

        private IPatchGenerator[] GeneratePatchers()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{_tblName}");
            Console.ResetColor();

            int totalSegments = Segments.Length;

            // create array of patchers
            IPatchGenerator[] patchers = new IPatchGenerator[totalSegments];

            // generate a ListPatcher for each segment
            for (int segmentIndex = 0, original_currentOffset = 0, mod_currentOffset = 0; segmentIndex < totalSegments; segmentIndex++)
            {
                Segment currentSegment = Segments[segmentIndex];

                // store segment size bytes of original and modded
                byte[] original_SegmentSizeBytes = _originalBytes[original_currentOffset..(original_currentOffset + 4)];
                original_currentOffset += 4;
                byte[] mod_SegmentSizeBytes = _moddedBytes[mod_currentOffset..(mod_currentOffset + 4)];
                mod_currentOffset += 4;

                // reverse bytes if big endian
                if (_isBigEndian)
                {
                    Array.Reverse(original_SegmentSizeBytes);
                    Array.Reverse(mod_SegmentSizeBytes);
                }

                uint original_segmentSize = BitConverter.ToUInt32(original_SegmentSizeBytes);
                uint mod_segmentSize = BitConverter.ToUInt32(mod_SegmentSizeBytes);

                //Console.WriteLine($"Segment: {currentSegment.SegmentName}\nSegmentSize (Original): {original_segmentSize}\nSegmentSize (Modded): {mod_segmentSize}\nSectionOffset (Original): {original_currentOffset}\nSectionOffset (Modded): {mod_currentOffset}");

                //Console.WriteLine($"EntryPatch - Original Offset: {original_currentOffset}, Mod Offset: {mod_currentOffset}, Entry Size: {currentSegment.EntrySize}\n");
                patchers[segmentIndex] = new EntryPatches(new SegmentProps { 
                    Tbl = _tblName,
                    Index = segmentIndex,
                    Name = currentSegment.SegmentName,
                    EntrySize = currentSegment.EntrySize,
                    OriginalOffset = original_currentOffset,
                    ModOffset = mod_currentOffset,
                    OriginalSize = original_segmentSize,
                    ModSize = mod_segmentSize,
                });

                // update current offset
                original_currentOffset = (int)(original_currentOffset + original_segmentSize);
                mod_currentOffset = (int)(mod_currentOffset + mod_segmentSize);

                // adding padding if needed
                int original_padding = original_currentOffset % 16 == 0 ? 0 : 16 - (original_currentOffset % 16);
                int mod_padding = mod_currentOffset % 16 == 0 ? 0 : 16 - (mod_currentOffset % 16);

                original_currentOffset += original_padding;
                mod_currentOffset += mod_padding;
            }

            return patchers;
        }

        protected override IPatchGenerator[] Patchers => GeneratePatchers();

        protected abstract Segment[] Segments { get; }
    }
}
