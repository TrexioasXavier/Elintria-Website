namespace DocCheck_HumanoidBones
{
using Elintria;
using Elintria.Standard;

public class AnyRigHat : ElintriaScript
{
    public GameObject character;

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(character);
        string head = HumanoidBones.Find(m, "Head");
        if (head != null && m.BonePose(head, out Vector3 p, out Quaternion r)) { transform.position = p + r * Vector3.up * 0.2f; transform.rotation = r; }
    }
}
}
