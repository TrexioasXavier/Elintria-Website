namespace DocCheck_GUILayout
{
using Elintria;

public class DialogButtons : EditorWindow
{
    [MenuItem("Window/My Tools/Dialog Buttons")]
    static void Open() { GetWindow<DialogButtons>("Buttons"); }

    void OnGUI()
    {
        GUILayout.Label("Apply the changes?");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.Width(80))) Close();
        if (GUILayout.Button("OK", GUILayout.Width(80))) { Debug.Log("Applied"); Close(); }
        GUILayout.EndHorizontal();
    }
}
}
