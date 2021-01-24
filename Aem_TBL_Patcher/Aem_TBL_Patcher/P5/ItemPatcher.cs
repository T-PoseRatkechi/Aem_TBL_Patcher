using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class ItemPatcher : BasePatcher
    {
        public ItemPatcher(byte[] originalBytes, byte[] moddedBytes) : base(originalBytes, moddedBytes) { }

        protected override IPatchGenerator[] Patchers => new IPatchGenerator[]
        {
            new ListPatches(0, 64, "Accessory Item Data"),
            new ListPatches(16400, 48, "Armor Item Data"),
            new ListPatches(30864, 48, "Consumable Item Data"),
            new ListPatches(55456, 12, "KeyItems Item Data"),
            new ListPatches(58544, 44, "Materials Item Data"),
            new ListPatches(69824, 48, "MeleeWeapons Item Data"),
            new ListPatches(82128, 32, "Outfits Item Data"),
            new ListPatches(87968, 24, "SkillCards Item Data"),
            new ListPatches(97264, 52, "RangedWeapons Item Data"),
        };
    }
}