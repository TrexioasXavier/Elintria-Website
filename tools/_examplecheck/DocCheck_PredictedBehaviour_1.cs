namespace DocCheck_PredictedBehaviour
{
using Elintria;
using Elintria.Netcode;

public class SmoothRemote : PredictedBehaviour<SmoothRemote.In, SmoothRemote.St>
{
    public struct In { public float turn, move; }
    public struct St { public Vector3 pos; public float yaw; }

    protected override In GatherInput() => new In { turn = Input.GetAxisHorizontal(), move = Input.GetAxisVertical() };

    protected override void Simulate(In i, float dt)
    {
        transform.Rotate(0f, i.turn * 120f * dt, 0f);
        transform.position += transform.forward * i.move * 4f * dt;
    }

    protected override St CaptureState() => new St { pos = transform.position, yaw = transform.eulerAngles.y };
    protected override void ApplyState(St s) { transform.position = s.pos; transform.rotation = Quaternion.Euler(0f, s.yaw, 0f); }

    protected override St Interpolate(St a, St b, float t) =>
        new St { pos = Vector3.Lerp(a.pos, b.pos, t), yaw = Mathf.LerpAngle(a.yaw, b.yaw, t) };
}
}
