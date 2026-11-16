namespace DocCheck_ObjectLook
{
using Elintria;

public class TeamPaint : ElintriaScript
{
    public int team;

    public override void Start()
    {
        string mat = team == 0 ? "Assets/Materials/Red.elmat" : "Assets/Materials/Blue.elmat";
        ObjectLook.SetMaterials(gameObject, new[] { mat });
    }
}
}
