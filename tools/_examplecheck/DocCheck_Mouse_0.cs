namespace DocCheck_Mouse
{
using Elintria;

public class FpsLook : ElintriaScript
{
    public float sensitivity = 0.12f;
    float yaw, pitch;

    public override void Start() { Mouse.DisableCursor(); }

    public override void Update()
    {
        Vector2 d = Mouse.RecenterAndGetDelta();
        yaw += d.x * sensitivity;
        pitch = Mathf.Clamp(pitch - d.y * sensitivity, -85f, 85f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (Input.GetKeyDown(KeyCode.Escape)) Mouse.EnableCursor();
    }

    public override void OnDisable() { Mouse.EnableCursor(); }
}
}
