namespace DocCheck_EditorApplication
{
using System.IO;
using Elintria;

public static class ExportNames
{
    [MenuItem("Tools/Export Enemy Names")]
    static void Export()
    {
        string path = EditorApplication.ProjectFile("Assets/Data/enemies.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (var w = new StreamWriter(path))
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) w.WriteLine(g.name);
        AssetFiles.Refresh();
        Debug.Log("Wrote " + path);
    }
}
}
