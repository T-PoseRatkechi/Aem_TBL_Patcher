using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    class EncountPatcher : IPatcher
    {
        struct EncounterTBL
        {
            public UInt32 Size { get; set; }
            public Encounter Encounters { get; set; }
            public byte[] Remainder { get; set; }
        }

        struct Encounter
        {
            public byte[] Flags { get; set; }
            public UInt16 Field04 { get; set; }
            public UInt16 Field06 { get; set; }
            public byte[] Units { get; set; }
            public UInt16 FieldID { get; set; }
            public UInt16 RoomID { get; set; }
            public UInt16 Music { get; set; }
        }

        public EncountPatcher()
        {

        }

        public List<PatchEdit> GetPatches(byte[] originalBytes, byte[] moddedBytes)
        {
            List<PatchEdit> thePatches = new List<PatchEdit>();

            uint originalSize = BitConverter.ToUInt32(originalBytes[0..4]);
            uint moddedSize = BitConverter.ToUInt32(moddedBytes[0..4]);

            // handle first 4 bytes
            if (originalSize != moddedSize)
                thePatches.Add(new PatchEdit() { Offset = 0, BytesEdit = moddedBytes[0..4] });

            int encountersParsed = 0;

            // handle parsed chunk
            for (int currentByte = 4, totalBytes = (int)moddedSize; currentByte < totalBytes; )
            {
                encountersParsed++;

                Encounter originalEncounter = ParseEncounter(originalBytes[currentByte..(currentByte + 24)]);
                Encounter moddedEncounter = ParseEncounter(moddedBytes[currentByte..(currentByte + 24)]);

                if (!originalEncounter.Flags.SequenceEqual(moddedEncounter.Flags))
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + originalEncounter.Flags.Length)] });
                currentByte += originalEncounter.Flags.Length;
                if (originalEncounter.Field04 != moddedEncounter.Field04)
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + sizeof(UInt16))] });
                currentByte += sizeof(UInt16);
                if (originalEncounter.Field06 != moddedEncounter.Field06)
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + sizeof(UInt16))] });
                currentByte += sizeof(UInt16);
                if (!originalEncounter.Units.SequenceEqual(moddedEncounter.Units))
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + originalEncounter.Units.Length)] });
                currentByte += originalEncounter.Units.Length;
                if (originalEncounter.FieldID != moddedEncounter.FieldID)
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + sizeof(UInt16))] });
                currentByte += sizeof(UInt16);
                if (originalEncounter.RoomID != moddedEncounter.RoomID)
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + sizeof(UInt16))] });
                currentByte += sizeof(UInt16);
                if (originalEncounter.Music != moddedEncounter.Music)
                    thePatches.Add(new PatchEdit() { Offset = currentByte, BytesEdit = moddedBytes[currentByte..(currentByte + sizeof(UInt16))] });
                currentByte += sizeof(UInt16);
            }

            // handle mystery bytes like old method
            BytePatcher bytePatcher = new BytePatcher((int)(moddedSize + 4), moddedBytes.Length);
            bytePatcher.GenerateBytePatches(thePatches, originalBytes, moddedBytes);

            Console.WriteLine($"ENC - Encounters Parsed: {encountersParsed}, Total Patches: {thePatches.Count}");

            return thePatches;
        }

        private Encounter ParseEncounter(byte[] encounterBytes)
        {
            if (encounterBytes.Length < 24)
            {
                Console.WriteLine("Not enough bytes to parse encounter!");
                return new Encounter();
            }

            Encounter theEncounter = new Encounter() {
                Flags = encounterBytes[0..4],
                Field04 = BitConverter.ToUInt16(encounterBytes[4..6]),
                Field06 = BitConverter.ToUInt16(encounterBytes[6..8]),
                Units = encounterBytes[8..18],
                FieldID = BitConverter.ToUInt16(encounterBytes[18..20]),
                RoomID = BitConverter.ToUInt16(encounterBytes[20..22]),
                Music = BitConverter.ToUInt16(encounterBytes[22..24])
            };

            return theEncounter;
        }
    }
}
