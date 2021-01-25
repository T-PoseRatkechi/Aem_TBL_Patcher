using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class ItemPatcher : AutoBasePatcher
    {
        public ItemPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(64, "Accessory Item Data"),
            new Segment(48, "Armor Item Data"),
            new Segment(48, "Consumable Item Data"),
            new Segment(12, "KeyItems Item Data"),
            new Segment(44, "Materials Item Data"),
            new Segment(48, "MeleeWeapons Item Data"),
            new Segment(32, "Outfits Item Data"),
            new Segment(24, "SkillCards Item Data"),
            new Segment(52, "RangedWeapons Item Data"),
        };

        protected override bool isBigEndian => true;
    }
}