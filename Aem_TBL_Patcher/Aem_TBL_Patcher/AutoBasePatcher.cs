using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    abstract class AutoBasePatcher : BasePatcher
    {
        protected struct Segment
        {
            public int entrySize { get; }
            public string segmentName { get; }

            public Segment(int s, string n)
            {
                entrySize = s;
                segmentName = n;
            }
        }

        public AutoBasePatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        private IPatchGenerator[] GeneratePatchers()
        {
            int totalSegments = Segments.Length;

            // create array of patchers
            IPatchGenerator[] patchers = new IPatchGenerator[totalSegments];

            // generate a ListPatcher for each segment
            for (int segmentIndex = 0, currentOffset = 0; segmentIndex < totalSegments; segmentIndex++)
            {
                Segment currentSegment = Segments[segmentIndex];

                // store segment size bytes
                byte[] segmentSizeBytes = _moddedBytes[currentOffset..(currentOffset + 4)];

                // reverse bytes if big endian
                if (isBigEndian)
                    Array.Reverse(segmentSizeBytes);

                uint segmentSize = BitConverter.ToUInt32(segmentSizeBytes);

                Console.WriteLine($"Segment: {currentSegment.segmentName}, SegmentSize: {segmentSize}, SectionOffset: {currentOffset + 4}");

                Console.WriteLine($"EntryPatch Created: Offset: {currentOffset}, ItemSize: {currentSegment.entrySize}\n");
                patchers[segmentIndex] = new EntryPatches(tblName, currentSegment.segmentName, segmentIndex, currentOffset + 4, segmentSize, currentSegment.entrySize);

                // update current offset
                currentOffset = (int)(currentOffset + segmentSize + segmentSizeBytes.Length);

                int padding = currentOffset % 16 == 0 ? 0 : 16 - (currentOffset % 16);
                currentOffset += padding;
            }

            return patchers;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }

        protected override IPatchGenerator[] Patchers => GeneratePatchers();

        protected abstract bool isBigEndian { get; }
        protected abstract string tblName { get; }

        protected abstract Segment[] Segments { get; }
    }
}
