namespace DocCheck_Bounds
{
using Elintria;

public class SafeZone : ElintriaScript
{
    public Vector3 size = new Vector3(10f, 4f, 10f);
    public GameObject player;

    public override void Update()
    {
        Bounds zone = new Bounds(transform.position, size);
        if (player != null && zone.Contains(player.transform.position))
            ScreenGUI.Text(20, 20, "Safe");
    }
}
}
