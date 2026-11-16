namespace DocCheck_LSystem
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class LineTree : ElintriaScript
{
    float[] data;
    int count;

    public override void Start()
    {
        var segs = LSystem.Turtle(LSystem.Expand("F", new Dictionary<char, string> { { 'F', "F[+F]F[-F]F" } }, 3), 0.1f, 25.7f);
        count = segs.Count;
        data = new float[count * 10];
        for (int i = 0; i < count; i++)
        {
            Vector3 a = transform.position + (Vector3)segs[i].a, b = transform.position + (Vector3)segs[i].b;
            float[] l = { a.x, a.y, a.z, b.x, b.y, b.z, 0.4f, 0.8f, 0.3f, 1f };
            l.CopyTo(data, i * 10);
        }
    }

    public override void Update() { Lines3D.Submit(data, count, 2f); }
}
}
