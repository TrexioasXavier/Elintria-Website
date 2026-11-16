namespace DocCheck_MessageType
{
using Elintria;

public class Note : EditorWindow
{
    [MenuItem("Window/My Tools/Note")]
    static void Open() { GetWindow<Note>("Note"); }
    void OnGUI() { EditorGUILayout.HelpBox("Remember to bake the nav mesh after editing the level.", MessageType.None); }
}
}
