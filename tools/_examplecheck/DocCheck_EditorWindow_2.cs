namespace DocCheck_EditorWindow
{
using Elintria;

public class PlayStats : EditorWindow
{
    [MenuItem("Window/My Tools/Play Stats")]
    static void Open() { GetWindow<PlayStats>("Play Stats"); }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Window size", width + " x " + height);
        EditorGUILayout.LabelField("Time", Time.time.ToString("0.0"));
        EditorGUILayout.LabelField("Rigidbodies", GameObject.FindObjectsOfType<Rigidbody>().Length.ToString());
        if (EditorGUILayout.Button("Close")) Close();
    }
}
}
