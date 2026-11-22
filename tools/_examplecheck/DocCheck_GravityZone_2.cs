namespace DocCheck_GravityZone
{
using Elintria;
using Elintria.Standard;

public class ZeroGWarning : ElintriaScript
{
    public override void Update()
    {
        foreach (GravityZone z in FindObjectsOfType<GravityZone>())
            if (z.Contains(transform.position) && z.gravityScale < 0.1f)
                ScreenGUI.Text(20f, 120f, "No gravity", null, new Color(0.7f, 0.6f, 1f, 1f));
    }
}
}
