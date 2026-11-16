namespace DocCheck_Mouse
{
using Elintria;

public class MouseTurret : ElintriaScript
{
    public float speed = 0.2f;
    float yaw;

    public override void Start() { Mouse.DisableCursor(); }

    public override void Update()
    {
        Vector2 d = Mouse.RecenterAndGetDelta();
        if (Mathf.Abs(d.x) > 1f) yaw += d.x * speed;   // ignore tiny jitter
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
}
