namespace DocCheck_ThrowableUser
{
using Elintria;
using Elintria.Standard;

public class DecoysOnly : ElintriaScript
{
    public override void Start()
    {
        ThrowableUser t = GetComponent<ThrowableUser>();
        t.startingCounts = "decoy:5|smoke:1";
        t.selected = 4;
    }
}
}
