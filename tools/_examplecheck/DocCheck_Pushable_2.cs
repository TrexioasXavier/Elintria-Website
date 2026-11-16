namespace DocCheck_Pushable
{
using Elintria;
using Elintria.Standard;

public class BlockOnMark : ElintriaScript
{
    public GameObject mark;

    public override void Update()
    {
        if (mark != null && Vector3.Distance(transform.position, mark.transform.position) < 0.3f) StdVars.Set("block_puzzle", "1");
    }
}
}
