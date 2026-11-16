namespace DocCheck_Space
{
using Elintria;

public class Nudge : ElintriaScript
{
    public bool local = true;

    public void By(Vector3 offset)
    {
        transform.Translate(offset, local ? Space.Self : Space.World);
    }
}
}
