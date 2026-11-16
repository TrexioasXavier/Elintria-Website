namespace DocCheck_Vector4
{
using Elintria;

public class MatrixPoint : ElintriaScript
{
    public override void Start()
    {
        Matrix4x4 m = transform.localToWorldMatrix;
        Vector4 p = m * new Vector4(0f, 1f, 0f, 1f);   // a point one unit above our origin
        Vector4 d = m * new Vector4(0f, 0f, 1f, 0f);   // our forward direction
        Debug.Log("World point " + (Vector3)p + ", world direction " + (Vector3)d);
    }
}
}
