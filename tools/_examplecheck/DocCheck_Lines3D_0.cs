namespace DocCheck_Lines3D
{
using Elintria;

public class ArcPreview : ElintriaScript
{
    public float throwSpeed = 12f;
    const int Steps = 30;
    readonly float[] l = new float[Steps * 10];

    public override void Update()
    {
        Vector3 p = transform.position, v = transform.forward * throwSpeed;
        for (int i = 0; i < Steps; i++)
        {
            Vector3 n = p + v * 0.05f;
            v += Physics.gravity * 0.05f;
            int o = i * 10;
            l[o] = p.x; l[o + 1] = p.y; l[o + 2] = p.z; l[o + 3] = n.x; l[o + 4] = n.y; l[o + 5] = n.z;
            l[o + 6] = 1; l[o + 7] = 1; l[o + 8] = 0.2f; l[o + 9] = 1;
            p = n;
        }
        Lines3D.Submit(l, Steps, 2f);
    }
}
}
