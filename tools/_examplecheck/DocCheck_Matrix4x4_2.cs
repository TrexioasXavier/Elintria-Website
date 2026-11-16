namespace DocCheck_Matrix4x4
{
using Elintria;

public class MirrorCheck : ElintriaScript
{
    public override void Start()
    {
        Matrix4x4 lift = Matrix4x4.Translate(new Vector3(0f, 2f, 0f));
        Matrix4x4 spin = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
        Matrix4x4 flip = Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
        Matrix4x4 all = lift * spin * flip;                 // flip, then spin, then lift

        Debug.Log("Mirrored: " + (all.determinant < 0f));
        Debug.Log("Forward becomes " + all.MultiplyVector(Vector3.forward));
        Debug.Log("Undo check: " + (all.inverse * all).MultiplyPoint3x4(new Vector3(1f, 2f, 3f)));
    }
}
}
