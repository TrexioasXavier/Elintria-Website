namespace DocCheck_Buoyancy
{
using Elintria;
using Elintria.Standard;

public class Barge : ElintriaScript
{
    public override void Start()
    {
        Buoyancy b = GetComponent<Buoyancy>();
        b.floatPoints.Clear();
        for (int x = -1; x <= 1; x += 2)
            for (int z = -2; z <= 2; z += 2)
                b.floatPoints.Add(new Vector3(x * 0.45f, -0.4f, z * 0.2f));
        b.buoyancy = 3f;
        b.waterDrag = 1.5f;
    }
}
}
