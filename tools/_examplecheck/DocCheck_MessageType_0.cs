namespace DocCheck_MessageType
{
using Elintria;

public class Validator : EditorWindow
{
    [MenuItem("Window/My Tools/Validator")]
    static void Open() { GetWindow<Validator>("Validator"); }

    void OnGUI()
    {
        bool hasPlayer = GameObject.FindWithTag("Player") != null;
        bool hasLight = GameObject.FindObjectsOfType<Light>().Length > 0;
        EditorGUILayout.HelpBox(hasPlayer ? "Player found." : "No object is tagged Player.", hasPlayer ? MessageType.Info : MessageType.Error);
        if (!hasLight) EditorGUILayout.HelpBox("The scene has no lights.", MessageType.Warning);
    }
}
}
