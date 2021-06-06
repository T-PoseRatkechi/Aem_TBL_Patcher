using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Segments
{
    public struct Segment
    {
        public int EntrySize { get; }
        public string SegmentName { get; }
        public bool UseEntries { get; }

        public Segment(int entrySize, string segmentName)
        {
            EntrySize = entrySize;
            SegmentName = segmentName;
            UseEntries = true;
        }

        public Segment(string segmentName)
        {
            EntrySize = -1;
            SegmentName = segmentName;
            UseEntries = false;
        }
    }

    public struct SegmentProps
    {
        public string Tbl { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int EntrySize { get; set; }
        public int OriginalOffset { get; set; }
        public int ModOffset { get; set; }
        public uint OriginalSize { get; set; }
        public uint ModSize { get; set; }
        public bool Itemtbl { get; set; }
    }
}
