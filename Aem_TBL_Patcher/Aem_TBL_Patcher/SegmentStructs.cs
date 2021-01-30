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

        public Segment(int s, string n)
        {
            EntrySize = s;
            SegmentName = n;
            UseEntries = true;
        }

        public Segment(string n)
        {
            EntrySize = -1;
            SegmentName = n;
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
    }
}
