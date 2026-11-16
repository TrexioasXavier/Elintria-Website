namespace DocCheck_Rng
{
using Elintria;
using Elintria.Procedural;

public class TreeHeights : ElintriaScript
{
    public override void Start()
    {
        var rng = new Rng(7, 3);   // seed 7, stream 3: separate from other uses of seed 7
        foreach (Transform tree in transform)
            tree.localScale = Vector3.one * Mathf.Max(0.3f, rng.Gaussian(1f, 0.2f));
    }
}
}
