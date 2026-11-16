namespace DocCheck_RuntimeMesh
{
using Elintria;

public class Pulsing : ElintriaScript
{
    RuntimeMesh mesh;
    Vector3[] baseVerts = { new Vector3(0, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0) };

    public override void Start()
    {
        mesh = new RuntimeMesh();
        mesh.SetTriangles(new[] { 0, 1, 2 });
        RuntimeMesh.Assign(gameObject, mesh);
    }

    public override void Update()
    {
        float s = 1f + 0.2f * Mathf.Sin(Time.time * 3f);
        Vector3[] v = new Vector3[3];
        for (int i = 0; i < 3; i++) v[i] = baseVerts[i] * s;
        mesh.SetVertices(v);
    }

    public override void OnDestroy() { RuntimeMesh.Assign(gameObject, null); mesh.Release(); }
}
}
