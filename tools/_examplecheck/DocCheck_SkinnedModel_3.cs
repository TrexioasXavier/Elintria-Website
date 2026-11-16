namespace DocCheck_SkinnedModel
{
using Elintria;

public class BoneLister : ElintriaScript
{
    public override void Start()
    {
        SkinnedModel m = SkinnedModel.Get(gameObject);
        string[] bones = m.BoneNames(out int[] parents);
        for (int i = 0; i < bones.Length; i++)
            Debug.Log(bones[i] + (parents[i] >= 0 ? " <- " + bones[parents[i]] : " (root)"));
    }
}
}
