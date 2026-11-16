namespace DocCheck_AssetFiles
{
using Elintria;

public static class MaterialList
{
    [MenuItem("Tools/List Materials")]
    static void List()
    {
        foreach (string m in AssetFiles.Find(".elmat")) Debug.Log(m);
    }
}
}
