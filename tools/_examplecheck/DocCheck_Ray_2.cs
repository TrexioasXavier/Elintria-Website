namespace DocCheck_Ray
{
using Elintria;

public class GroundPicker : ElintriaScript
{
    public override void Update()
    {
        Ray r = new Ray(transform.position, -transform.forward);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(r, out float enter))
            Debug.DrawRay(r.GetPoint(enter), Vector3.up * 2f, Color.green);
    }
}
}
