namespace DocCheck_ClimbableSurface
{
using Elintria;
using Elintria.Standard;

public class ClimbMarkers : ElintriaScript
{
    public override void Update()
    {
        foreach (ClimbableSurface s in FindObjectsOfType<ClimbableSurface>())
            StdHud.Marker(s.transform.position + Vector3.up * 2f, new Color(0.5f, 1f, 0.5f, 1f), -1f);
    }
}
}
