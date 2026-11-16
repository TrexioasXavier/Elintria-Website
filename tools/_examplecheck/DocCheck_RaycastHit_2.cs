namespace DocCheck_RaycastHit
{
using Elintria;

public class RangeFinder : ElintriaScript
{
    public override void Update()
    {
        string text = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 500f)
            ? hit.gameObject.name + " - " + hit.distance.ToString("0.0") + " m"
            : "no target";
        ScreenGUI.Text(20, 20, text);
    }
}
}
