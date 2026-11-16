namespace DocCheck_Transform
{
using Elintria;

public class SimpleWalker : ElintriaScript
{
    public float moveSpeed = 4f;
    public float turnSpeed = 120f;   // degrees a second per unit of mouse movement

    public override void Update()
    {
        float forward = Input.GetAxisVertical();     // W/S
        float strafe = Input.GetAxisHorizontal();    // A/D
        transform.Translate(new Vector3(strafe, 0f, forward) * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButton(1))
            transform.Rotate(0f, Input.mouseDelta.x * turnSpeed * Time.deltaTime, 0f);
    }
}
}
