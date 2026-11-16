namespace DocCheck_Input
{
using Elintria;

public class AxisMover : ElintriaScript
{
    public float speed = 6f;

    public override void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        if (move.sqrMagnitude > 1f) move = move.normalized;
        transform.position += move * speed * Time.deltaTime;

        if (Input.GetButtonDown("Jump")) Debug.Log("Jump!");
    }
}
}
