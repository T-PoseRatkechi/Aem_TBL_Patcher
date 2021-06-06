using Aem_TBL_Patcher.Segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher.Patchers.P4G
{
    class ItemPatcher : ItemtblPatcher
    {
        public ItemPatcher() : base("ITEMTBL", false) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(68, "ItemData"),
            new Segment(24, "ItemNames")
        };
    }
}
