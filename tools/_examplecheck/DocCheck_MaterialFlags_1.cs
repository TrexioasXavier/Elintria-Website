namespace DocCheck_MaterialFlags
{
using Elintria;

public class LeafFlags : ElintriaScript
{
    public static void MakeLeaves(MeshInstance i, int leafTexture)
    {
        i.SetMaterial(leafTexture, 1f, 1f, 1f, 1f, MaterialFlags.AlphaTest | MaterialFlags.DoubleSided);
    }
}
}
