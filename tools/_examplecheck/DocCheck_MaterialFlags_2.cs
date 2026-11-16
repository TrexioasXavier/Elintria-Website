namespace DocCheck_MaterialFlags
{
using Elintria;

public class ViewModelFlags : ElintriaScript
{
    public static void MakeViewModel(MeshInstance i, int tex)
    {
        i.SetMaterial(tex, 1f, 1f, 1f, 1f, MaterialFlags.Overlay | MaterialFlags.NoRenderTexture);
    }
}
}
