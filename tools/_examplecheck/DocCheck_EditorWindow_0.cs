namespace DocCheck_EditorWindow
{
using Elintria;

public class RenameTool : EditorWindow
{
    string prefix = "Enemy_";
    int start = 1;

    [MenuItem("Window/My Tools/Rename Tool")]
    static void Open() { GetWindow<RenameTool>("Rename Tool"); }

    void OnGUI()
    {
        EditorGUILayout.Header("Batch rename");
        prefix = EditorGUILayout.TextField("Prefix", prefix);
        start = EditorGUILayout.IntField("Start at", start);
        if (EditorGUILayout.Button("Rename objects named 'Enemy'"))
        {
            int n = start;
            foreach (GameObject g in GameObject.FindObjectsWithNamePrefix("Enemy"))
                g.name = prefix + (n++);
        }
    }
}
}
