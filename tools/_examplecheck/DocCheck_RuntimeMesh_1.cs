namespace DocCheck_RuntimeMesh
{
using Elintria;

public class NoiseHills : ElintriaScript
{
    public int size = 64;
    public float scale = 0.08f;
    public float height = 6f;
    RuntimeMesh mesh;

    public override void Start()
    {
        int n = size + 1;
        Vector3[] v = new Vector3[n * n];
        Vector2[] uv = new Vector2[n * n];
        for (int z = 0; z < n; z++)
            for (int x = 0; x < n; x++)
            {
                v[z * n + x] = new Vector3(x, Mathf.PerlinNoise(x * scale, z * scale) * height, z);
                uv[z * n + x] = new Vector2(x / (float)size, z / (float)size);
            }
        int[] tri = new int[size * size * 6];
        int t = 0;
        for (int z = 0; z < size; z++)
            for (int x = 0; x < size; x++)
            {
                int i = z * n + x;
                tri[t++] = i; tri[t++] = i + n; tri[t++] = i + 1;
                tri[t++] = i + 1; tri[t++] = i + n; tri[t++] = i + n + 1;
            }
        mesh = new RuntimeMesh();
        mesh.SetVertices(v);
        mesh.SetUVs(uv);
        mesh.SetTriangles(tri);
        RuntimeMesh.Assign(gameObject, mesh);
    }

    public override void OnDestroy() { if (mesh != null) mesh.Release(); }
}
}
