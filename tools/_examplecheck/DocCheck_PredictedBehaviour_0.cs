namespace DocCheck_PredictedBehaviour
{
using Elintria;
using Elintria.Netcode;

public class NetPlayer : PredictedBehaviour<NetPlayer.In, NetPlayer.St>
{
    public struct In { public float x, z; public bool jump; }
    public struct St { public Vector3 pos; public float vy; }

    public float speed = 5f;
    float vy;

    protected override In GatherInput() =>
        new In { x = Input.GetAxisHorizontal(), z = Input.GetAxisVertical(), jump = Input.GetKey(KeyCode.Space) };

    protected override void Simulate(In i, float dt)
    {
        bool grounded = transform.position.y <= 0.001f;
        if (grounded && i.jump) vy = 6f;
        vy -= 20f * dt;
        Vector3 p = transform.position + new Vector3(i.x, 0f, i.z) * speed * dt + Vector3.up * vy * dt;
        if (p.y < 0f) { p.y = 0f; vy = 0f; }
        transform.position = p;
    }

    protected override St CaptureState() => new St { pos = transform.position, vy = vy };
    protected override void ApplyState(St s) { transform.position = s.pos; vy = s.vy; }
}
}
