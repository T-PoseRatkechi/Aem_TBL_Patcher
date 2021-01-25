using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher
{
    class EntryPatches : IPatchGenerator
    {
        /*
        readonly string _tblName = String.Empty;
        readonly string _segmentName = String.Empty;
        readonly int _segmentIndex = 0;
        readonly int _originalOffset = 0;
        readonly int _modOffset = 0;
        readonly uint _originalSegmentSize = 0;
        readonly uint _modSegmentSize = 0;
        readonly int _entrySize = 0;

        public EntryPatches(string tblName, string segmentName, int segmentIndex, int modOffset, uint modSegmentSize, int itemSize)
        {
            _tblName = tblName;
            _segmentName = segmentName;
            _segmentIndex = segmentIndex;
            _modOffset = modOffset;
            _modSegmentSize = modSegmentSize;
            _modOffset = modOffset;
            _modSegmentSize = modSegmentSize;
            _entrySize = itemSize;
        }
        */

        readonly SegmentProps _segmentProps;

        public EntryPatches(SegmentProps segmentProps)
        {
            _segmentProps = segmentProps;
        }

        public void GeneratePatches(List<PatchEdit> patches, byte[] originalBytes, byte[] moddedBytes)
        {
            int patchesCount = 0;

            for (int currentEntry = 0, numEntries = (int)(_segmentProps.ModSize / _segmentProps.EntrySize); currentEntry < numEntries; currentEntry++)
            {
                int currentOffset = _segmentProps.ModOffset + (currentEntry * _segmentProps.EntrySize);

                byte[] originalElement = originalBytes[currentOffset..(currentOffset + _segmentProps.EntrySize)];
                byte[] moddedElement = moddedBytes[currentOffset..(currentOffset + _segmentProps.EntrySize)];

                if (!originalElement.SequenceEqual(moddedElement))
                {
                    patches.Add(new PatchEdit {
                        comment = $"Segment: {_segmentProps.Name}, Entry Index: {currentEntry}",
                        tbl = _segmentProps.Tbl,
                        section = _segmentProps.Index,
                        offset = (currentEntry * _segmentProps.EntrySize),
                        data = ByteArrayToString(moddedElement),
                    });

                    Console.WriteLine($"Entry Patch\nTBL: {_segmentProps.Tbl}\nSegmentIndex: {_segmentProps.Index}\nOffset: {currentEntry * _segmentProps.EntrySize}\nData: {ByteArrayToString(moddedElement)}");

                    patchesCount++;
                }
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }
    }
}
