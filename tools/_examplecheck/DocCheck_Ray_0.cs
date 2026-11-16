namespace DocCheck_Ray
{
using Elintria;

public class CenterRay : ElintriaScript
{
    public override void Update()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            ScreenGUI.Text(20, 20, "Looking at " + hit.gameObject.name);
    }
}
}
