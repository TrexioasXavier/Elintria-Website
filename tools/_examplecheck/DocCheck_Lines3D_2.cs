namespace DocCheck_Lines3D
{
using System.Collections.Generic;
using Elintria;

public class WireBox : ElintriaScript
{
    public Vector3 size = Vector3.one;

    public override void Update()
    {
        Vector3 h = size * 0.5f, c = transform.position;
        var lines = new List<float>();
        int[,] e = { {0,1},{1,3},{3,2},{2,0},{4,5},{5,7},{7,6},{6,4},{0,4},{1,5},{2,6},{3,7} };
        Vector3 Corner(int i) => c + new Vector3((i & 1) != 0 ? h.x : -h.x, (i & 2) != 0 ? h.y : -h.y, (i & 4) != 0 ? h.z : -h.z);
        for (int i = 0; i < 12; i++)
        {
            Vector3 a = Corner(e[i, 0]), b = Corner(e[i, 1]);
            lines.AddRange(new[] { a.x, a.y, a.z, b.x, b.y, b.z, 0f, 1f, 0.5f, 1f });
        }
        Lines3D.Submit(lines.ToArray(), 12, 1.5f);
    }
}
}
