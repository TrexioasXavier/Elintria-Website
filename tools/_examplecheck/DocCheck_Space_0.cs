namespace DocCheck_Space
{
using Elintria;

public class SpaceDemo : ElintriaScript
{
    public override void Update()
    {
        // Forward the way the object faces:
        transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        // Always drift up in the world, even if the object is tilted:
        transform.Translate(Vector3.up * 0.2f * Time.deltaTime, Space.World);
    }
}
}
