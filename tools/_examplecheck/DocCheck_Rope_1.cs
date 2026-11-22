namespace DocCheck_Rope
{
using Elintria;

public class RopeCutter : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) GetComponent<Rope>().DetachEnd();
    }
}
}
