namespace DocCheck_Poisson
{
using Elintria;
using Elintria.Procedural;

public class PoissonCount : ElintriaScript
{
    public override void Start()
    {
        foreach (float r in new[] { 2f, 4f, 8f })
            Debug.Log("radius " + r + ": " + Poisson.Sample(50f, 50f, r, 1).Count + " points");
    }
}
}
