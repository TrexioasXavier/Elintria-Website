namespace DocCheck_ScriptCamera
{
using Elintria;

public class FreeFly : ElintriaScript
{
    public float speed = 10f;
    Vector3 pos;
    float yaw, pitch;

    public override void Start() { pos = transform.position; }

    public override void Update()
    {
        if (Input.GetMouseButton(1)) { yaw += Input.mouseDelta.x * 0.2f; pitch = Mathf.Clamp(pitch - Input.mouseDelta.y * 0.2f, -89f, 89f); }
        Quaternion q = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 fwd = q * Vector3.forward;
        pos += (fwd * Input.GetAxisVertical() + (q * Vector3.right) * Input.GetAxisHorizontal()) * speed * Time.deltaTime;
        ScriptCamera.Set(pos, fwd, Vector3.up, 70f, 0.1f, 500f);
    }

    public override void OnDisable() { ScriptCamera.Clear(); }
}
}
