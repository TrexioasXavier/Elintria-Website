namespace DocCheck_NoiseEvent
{
using Elintria;
using Elintria.Standard;

public class NoiseListener : ElintriaScript
{
    public float hearing = 1f;

    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (StdNoise.Heard(transform.position, hearing, 2f, out n))
        {
            Vector3 dir = n.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
}
