namespace DocCheck_Vector3
{
using Elintria;

public class BounceBall : ElintriaScript
{
    public Vector3 velocity = new Vector3(4f, 0f, 3f);
    public float radius = 0.5f;

    public override void Update()
    {
        Vector3 step = velocity * Time.deltaTime;
        if (Physics.Raycast(transform.position, velocity, out RaycastHit hit, step.magnitude + radius))
            velocity = Vector3.Reflect(velocity, hit.normal) * 0.9f;   // lose a little energy
        transform.position += velocity * Time.deltaTime;
    }
}
}
