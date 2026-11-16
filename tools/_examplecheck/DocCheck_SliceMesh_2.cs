namespace DocCheck_SliceMesh
{
using Elintria;
using Elintria.Standard;

public class PieceSizes : ElintriaScript
{
    public override void Start()
    {
        SliceMesh s = SliceMesh.Sphere(16);
        s.Split(new Vector3(0f, 0.3f, 0f), Vector3.up, out SliceMesh cap, out SliceMesh rest);
        Debug.Log("Cap size " + cap.Box().size + ", rest size " + rest.Box().size);
    }
}
}
