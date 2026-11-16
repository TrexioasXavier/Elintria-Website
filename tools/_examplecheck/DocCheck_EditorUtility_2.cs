namespace DocCheck_EditorUtility
{
using System.IO;
using Elintria;

public static class OverwriteCheck
{
    public static bool OkToWrite(string path) =>
        !File.Exists(path) || EditorUtility.DisplayDialog("File exists", Path.GetFileName(path) + " already exists. Replace it?", "Replace", "Keep");
}
}
