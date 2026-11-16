namespace DocCheck_EditorWindow
{
using Elintria;

public class TextureBrowser : EditorWindow
{
    string filter = "";
    string selected;

    [MenuItem("Window/My Tools/Texture Browser")]
    static void Open() { GetWindow<TextureBrowser>("Textures"); }

    void OnGUI()
    {
        filter = EditorGUILayout.SearchField(filter, "texfilter");
        EditorGUILayout.BeginScrollView(0f, "texlist");
        foreach (string path in AssetFiles.Find(".png", ".jpg"))
        {
            if (filter.Length > 0 && !path.ToLowerInvariant().Contains(filter.ToLowerInvariant())) continue;
            if (EditorGUILayout.ListRow(path, path == selected) > 0) selected = path;
        }
        EditorGUILayout.EndScrollView();
        if (selected != null) EditorGUILayout.Image(selected, 128f);
    }
}
}
