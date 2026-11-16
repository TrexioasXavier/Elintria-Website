namespace DocCheck_EditorApplication
{
using Elintria;

public class ProjectInfo : EditorWindow
{
    [MenuItem("Window/My Tools/Project Info")]
    static void Open() { GetWindow<ProjectInfo>("Project"); }
    void OnGUI() { EditorGUILayout.LabelField("Folder", EditorApplication.projectPath); }
}
}
