using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class EffectPatcher : AutoBasePatcher
{
    public EffectPatcher() : base("EFFECT", false) { }

    protected override Segment[] Segments => new Segment[]
{
            new Segment("EFFECTUnknown1"),
    };
}
