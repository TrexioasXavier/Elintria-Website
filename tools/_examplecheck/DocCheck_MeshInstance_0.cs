namespace DocCheck_MeshInstance
{
using Elintria;

public class SpinCrate : ElintriaScript
{
    public GameObject meshSource;   // a script that built a GpuMesh exposes its id
    MeshInstance inst;
    float[] m = new float[16];

    public void Init(GpuMesh mesh, int texture)
    {
        inst = new MeshInstance(mesh);
        inst.SetMaterial(texture, 1f, 1f, 1f, 1f, MaterialFlags.None);
    }

    public override void Update()
    {
        if (inst == null) return;
        Matrix4x4 trs = Matrix4x4.TRS(transform.position, Quaternion.Euler(0f, Time.time * 90f, 0f), Vector3.one);
        for (int i = 0; i < 16; i++) m[i] = trs[i];
        inst.SetMatrix(m);
    }
}
}
