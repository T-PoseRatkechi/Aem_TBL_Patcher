using Aem_TBL_Patcher.Segments;

namespace Aem_TBL_Patcher.Patchers.P3P;

internal class SkillPatcher : AutoBasePatcher
{
    public SkillPatcher() : base("SKILL", false) { }

    protected override Segment[] Segments => new Segment[]
    {
            new Segment(2, "Skill Elements"),
            new Segment(40, "Active Skill Data")
    };
}
