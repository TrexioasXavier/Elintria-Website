namespace DocCheck_GpuMesh
{
using System.Collections.Generic;
using Elintria;

public class CubeField : ElintriaScript
{
    readonly List<MeshInstance> cubes = new List<MeshInstance>();
    GpuMesh box;

    public override void Start()
    {
        box = BuildBox();
        for (int x = 0; x < 30; x++)
            for (int z = 0; z < 30; z++)
            {
                var i = new MeshInstance(box);
                i.SetPosition(x * 1.5f, Mathf.PerlinNoise(x * 0.2f, z * 0.2f) * 3f, z * 1.5f);
                i.SetColor(0.3f, 0.6f + x / 90f, 0.4f, 1f);
                cubes.Add(i);
            }
    }

    static GpuMesh BuildBox()
    {
        // 8 corners, 12 triangles, wound anticlockwise from outside.
        float[] p = { -.5f,-.5f,-.5f, .5f,-.5f,-.5f, .5f,.5f,-.5f, -.5f,.5f,-.5f, -.5f,-.5f,.5f, .5f,-.5f,.5f, .5f,.5f,.5f, -.5f,.5f,.5f };
        int[] t = { 0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4, 3,6,2, 3,7,6, 0,4,7, 0,7,3, 1,2,6, 1,6,5 };
        var m = new GpuMesh();
        m.SetData(p, null, null, null, 8, t, t.Length);
        return m;
    }

    public override void OnDestroy() { foreach (var c in cubes) c.Destroy(); box.Release(); }
}
}
