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
            new ListPatches(0, 64, "Accessory Item Data", true),
            new ListPatches(16400, 48, "Armor Item Data", true),
            new ListPatches(30864, 48, "Consumable Item Data", true),
            new ListPatches(55456, 12, "KeyItems Item Data", true),
            new ListPatches(58544, 44, "Materials Item Data", true),
            new ListPatches(69824, 48, "MeleeWeapons Item Data", true),
            new ListPatches(82128, 32, "Outfits Item Data", true),
            new ListPatches(87968, 24, "SkillCards Item Data", true),
            new ListPatches(97264, 52, "RangedWeapons Item Data", true),
        };
    }
}