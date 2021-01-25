using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class ItemPatcher : AutoBasePatcher
    {
        public ItemPatcher() : base("ITEM", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(64, "Accessory Item"),
            new Segment(48, "Armor Item"),
            new Segment(48, "Consumable Item"),
            new Segment(12, "KeyItems Item"),
            new Segment(44, "Materials Item"),
            new Segment(48, "MeleeWeapons Item"),
            new Segment(32, "Outfits Item"),
            new Segment(24, "SkillCards Item"),
            new Segment(52, "RangedWeapons Item"),
        };
    }
}