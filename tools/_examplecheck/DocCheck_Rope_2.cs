namespace DocCheck_Rope
{
using Elintria;

public class SnapRope : ElintriaScript
{
    public override void Start()
    {
        Rope r = GetComponent<Rope>();
        r.breakForce = 800f;
    }

    public override void Update()
    {
        Rope r = GetComponent<Rope>();
        if (r.broken) Debug.Log("Snap!");
        else if (r.tension > 600f) Debug.Log("The rope strains (" + r.tension.ToString("0") + " N)");
    }
}
}
