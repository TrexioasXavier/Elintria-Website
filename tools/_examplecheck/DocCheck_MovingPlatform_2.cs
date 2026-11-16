namespace DocCheck_MovingPlatform
{
using Elintria;
using Elintria.Standard;

public class Ferry : ElintriaScript
{
    public GameObject player;

    public override void Update()
    {
        MovingPlatform m = GetComponent<MovingPlatform>();
        bool aboard = player != null && Vector3.Distance(player.transform.position, transform.position) < 3f;
        if (aboard && !m.moving) m.Activate();
        else if (!aboard && m.moving) m.Deactivate();
    }
}
}
