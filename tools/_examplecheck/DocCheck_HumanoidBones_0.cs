namespace DocCheck_HumanoidBones
{
using Elintria;
using Elintria.Standard;

public class RigReport : ElintriaScript
{
    public override void Start()
    {
        var map = HumanoidBones.Map(SkinnedModel.Get(gameObject));
        foreach (string part in HumanoidBones.Parts)
            Debug.Log(part + " -> " + (map.TryGetValue(part, out string bone) ? bone : "(missing)"));
    }
}
}
