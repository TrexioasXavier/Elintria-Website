namespace DocCheck_EditorGUILayout
{
using Elintria;

public class MusicPicker : EditorWindow
{
    string track = "";

    [MenuItem("Window/My Tools/Music Picker")]
    static void Open() { GetWindow<MusicPicker>("Music"); }

    void OnGUI()
    {
        track = EditorGUILayout.AssetField("Track", track, ".ogg", ".wav");
        EditorGUILayout.LabelField("Chosen", track == "" ? "(none)" : track);
    }
}
}
