namespace DocCheck_EditorGUILayout
{
using Elintria;

public class TabbedTool : EditorWindow
{
    int tab;

    [MenuItem("Window/My Tools/Tabbed Tool")]
    static void Open() { GetWindow<TabbedTool>("Tabs"); }

    void OnGUI()
    {
        tab = EditorGUILayout.Toolbar(tab, new[] { "Spawn", "Clean up" });
        EditorGUILayout.Space(6f);
        if (tab == 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.Button("Cube")) GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (EditorGUILayout.Button("Sphere")) GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (EditorGUILayout.Button("Light")) new GameObject("Light").AddComponent<Light>();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("Removes every object whose name starts with 'Temp'.", MessageType.Warning);
            if (EditorGUILayout.Button("Delete temp objects"))
                foreach (GameObject g in GameObject.FindObjectsWithNamePrefix("Temp")) GameObject.Destroy(g);
        }
    }
}
}
