namespace DocCheck_Input
{
using Elintria;

public class MouseLook : ElintriaScript
{
    public float sensitivity = 0.15f;
    float yaw, pitch;

    public override void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 d = Input.mouseDelta;
            yaw += d.x * sensitivity;
            pitch = Mathf.Clamp(pitch - d.y * sensitivity, -80f, 80f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50f))
            Debug.Log("Clicked " + hit.gameObject.name + " at pointer " + Input.mousePosition);
    }
}
}
