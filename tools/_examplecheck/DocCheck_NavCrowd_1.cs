namespace DocCheck_NavCrowd
{
using Elintria;

public class CrowdFocus : ElintriaScript
{
    public override void Update() { NavCrowd.SetFocus(transform.position); }
}
}
