namespace DocCheck_Space
{
using Elintria;

public class PlaneControl : ElintriaScript
{
    public float rate = 60f;

    public override void Update()
    {
        float roll = -Input.GetAxisHorizontal() * rate * Time.deltaTime;
        float pitch = Input.GetAxisVertical() * rate * Time.deltaTime;
        transform.Rotate(new Vector3(pitch, 0f, roll), Space.Self);
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up, -rate * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, rate * Time.deltaTime, Space.World);
    }
}
}
