namespace DocCheck_AssetFiles
{
using System.IO;
using Elintria;

public static class GenerateData
{
    [MenuItem("Tools/Generate Item Files")]
    static void Generate()
    {
        for (int i = 0; i < 3; i++)
            File.WriteAllText(EditorApplication.ProjectFile("Assets/Items/item" + i + ".json"), "{ \"id\": \"item" + i + "\" }");
        AssetFiles.Refresh();
        Debug.Log(AssetFiles.Find(".json").Length + " json files now");
    }
}
}
