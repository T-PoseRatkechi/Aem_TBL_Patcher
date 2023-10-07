using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class EncountPatcher : AutoBasePatcher
{
    public EncountPatcher() : base("ENCOUNT", false) { }

    protected override Segment[] Segments => new Segment[]
    {
            new Segment(28, "Encounter"),
    };
}
