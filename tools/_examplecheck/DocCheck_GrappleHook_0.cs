namespace DocCheck_GrappleHook
{
using Elintria;
using Elintria.Standard;

public class GrapplePoints : ElintriaScript
{
    public override void Update()
    {
        GrappleHook g = GetComponent<GrappleHook>();
        if (!g.pulling) return;
        bool ok = false;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("GrapplePoint"))
            if (Vector3.Distance(p.transform.position, g.point) < 1.5f) ok = true;
        if (!ok) g.Release();
    }
}
}
