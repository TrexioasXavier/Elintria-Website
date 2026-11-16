namespace DocCheck_Vector3
{
using Elintria;

public class VisionCone : ElintriaScript
{
    public GameObject player;
    public float range = 15f;
    public float halfAngle = 40f;

    public bool CanSee()
    {
        if (player == null) return false;
        Vector3 to = player.transform.position - transform.position;
        if (to.sqrMagnitude > range * range) return false;
        return Vector3.Angle(transform.forward, to) < halfAngle;
    }

    public float SideOf()
    {
        // +1 when the player is on our right, -1 on our left.
        Vector3 to = (player.transform.position - transform.position).normalized;
        return Mathf.Sign(Vector3.Dot(transform.right, to));
    }

    public override void Update()
    {
        if (CanSee()) Debug.DrawLine(transform.position, player.transform.position, Color.red);
    }
}
}
