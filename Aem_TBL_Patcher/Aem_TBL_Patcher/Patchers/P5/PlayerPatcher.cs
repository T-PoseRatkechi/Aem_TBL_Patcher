using System;
using System.Collections.Generic;
using System.Text;
using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P5
{
    class PlayerPatcher : AutoBasePatcher
    {
        public PlayerPatcher() : base("PLAYER", true) { }

        protected override Segment[] Segments => new Segment[]
        {
            new Segment(4, "Exp Threshold"),
            new Segment(44, "HP_SP Per Level")
        };
    }
}