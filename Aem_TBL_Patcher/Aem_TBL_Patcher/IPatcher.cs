using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    interface IPatcher
    {
        List<PatchEdit> GetPatches();
    }
}
