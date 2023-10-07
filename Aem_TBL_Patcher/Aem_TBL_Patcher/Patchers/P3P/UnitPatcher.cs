using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class UnitPatcher : AutoBasePatcher
{
    public UnitPatcher() : base("UNIT", false) { }

    protected override Segment[] Segments => new Segment[]
    {
            new Segment(62, "Enemy Unit Stats"),
            new Segment(34, "Enemy Elemental Affinities"),
            new Segment(34, "Persona Elemental Affinities"),
    };
}
