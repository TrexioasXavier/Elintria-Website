namespace DocCheck_GUILayout
{
using Elintria;

public class PreviewArea : EditorWindow
{
    [MenuItem("Window/My Tools/Preview Area")]
    static void Open() { GetWindow<PreviewArea>("Preview"); }

    void OnGUI()
    {
        GUILayout.Label("Preview:");
        Rect r = EditorGUILayout.GetRect(120f, GUILayout.MinWidth(200));
        EditorGUI.DrawRect(r, new Color(0.1f, 0.1f, 0.12f));
        GUILayout.Space(8f);
        GUILayout.Button("Refresh", GUILayout.Height(28));
    }
}
}
