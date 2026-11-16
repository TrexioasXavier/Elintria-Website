namespace DocCheck_StdNoise
{
using Elintria;
using Elintria.Standard;

public class GuardDog : ElintriaScript
{
    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (StdNoise.Heard(transform.position, 2f, 0.5f, out n)) Debug.Log("Woof! Something at " + n.position);
    }
}
}
