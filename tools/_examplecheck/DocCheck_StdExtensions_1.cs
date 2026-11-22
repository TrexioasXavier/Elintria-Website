namespace DocCheck_StdExtensions
{
using Elintria;
using Elintria.Standard;

public class PlatformCheck : ElintriaScript
{
    public Transform player;

    public override void Update()
    {
        RaycastHit hit;
        if (player != null && Physics.Raycast(player.position, Vector3.down, out hit, 1.5f))
            if (hit.collider.transform.IsChildOf(transform)) Debug.Log("Player is on " + name);
    }
}
}
