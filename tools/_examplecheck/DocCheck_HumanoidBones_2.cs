namespace DocCheck_HumanoidBones
{
using Elintria;
using Elintria.Standard;

public static class IsHumanoid
{
    public static bool Check(SkinnedModel m) =>
        HumanoidBones.Find(m, "Hips") != null && HumanoidBones.Find(m, "Head") != null && HumanoidBones.Find(m, "LeftFoot") != null;
}
}
