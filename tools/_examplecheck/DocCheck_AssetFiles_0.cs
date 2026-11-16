namespace DocCheck_AssetFiles
{
using Elintria;

public class AssetCounts : EditorWindow
{
    [MenuItem("Window/My Tools/Asset Counts")]
    static void Open() { GetWindow<AssetCounts>("Assets"); }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Textures", AssetFiles.Find(".png", ".jpg", ".tga").Length.ToString());
        EditorGUILayout.LabelField("Models", AssetFiles.Find(".fbx", ".obj").Length.ToString());
        EditorGUILayout.LabelField("Sounds", AssetFiles.Find(".wav", ".ogg").Length.ToString());
    }
}
}
