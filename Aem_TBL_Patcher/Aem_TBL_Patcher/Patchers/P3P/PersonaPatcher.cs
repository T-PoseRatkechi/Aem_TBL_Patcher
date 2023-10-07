using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class PersonaPatcher : AutoBasePatcher
{
    public PersonaPatcher() : base("PERSONA", false) { }

    protected override Segment[] Segments => new Segment[]
    {
            new Segment(14, "Persona Stats"),
            new Segment(70, "Persona Skills and Stat Growths"),
            new Segment(622, "Party Personas"),
            new Segment(392, "Party LV UP Thresholds"),
            new Segment(2, "Persona Exist"),
            new Segment(4, "Persona Fusion")
    };
}
