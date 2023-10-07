using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class ModelPatcher : AutoBasePatcher
{
    public ModelPatcher() : base("MODEL", false) { }

    protected override Segment[] Segments => new Segment[]
    {
            new Segment("ModelUnknown1"),
    };
}
