namespace DocCheck_GUILayout
{
using Elintria;

public class TwoColumns : EditorWindow
{
    [MenuItem("Window/My Tools/Two Columns")]
    static void Open() { GetWindow<TwoColumns>("Columns"); }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(150));
        GUILayout.Label("Left column");
        GUILayout.Button("A");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Right column");
        GUILayout.Button("B");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
}
