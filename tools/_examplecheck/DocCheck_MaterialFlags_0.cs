namespace DocCheck_MaterialFlags
{
using Elintria;

public class GlassFlags : ElintriaScript
{
    public static void MakeGlass(MeshInstance i)
    {
        i.SetMaterial(0, 0.7f, 0.9f, 1f, 0.3f, MaterialFlags.Transparent | MaterialFlags.DoubleSided);
    }
}
}
