namespace DocCheck_Vector2
{
using Elintria;

public class TopDown2D : ElintriaScript
{
    public float speed = 5f;

    public override void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisHorizontal(), Input.GetAxisVertical());
        if (input.sqrMagnitude > 1f) input = input.normalized;
        Vector2 step = input * speed * Time.deltaTime;
        transform.position += new Vector3(step.x, step.y, 0f);
    }
}
}
