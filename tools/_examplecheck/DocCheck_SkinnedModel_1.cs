namespace DocCheck_SkinnedModel
{
using Elintria;

public class HatAttach : ElintriaScript
{
    public GameObject character;
    public string bone = "Head";
    public Vector3 offset = new Vector3(0f, 0.15f, 0f);

    public override void LateUpdate()
    {
        SkinnedModel m = SkinnedModel.Get(character);
        if (m != null && m.BonePose(bone, out Vector3 p, out Quaternion r))
        {
            transform.position = p + r * offset;
            transform.rotation = r;
        }
    }
}
}
