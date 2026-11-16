namespace DocCheck_EditorGUILayout
{
using Elintria;

public class LevelSettingsWindow : EditorWindow
{
    float gravity = -9.8f, fogEnd = 120f;
    int difficulty;
    bool night;
    bool advanced;

    [MenuItem("Window/My Tools/Level Settings")]
    static void Open() { GetWindow<LevelSettingsWindow>("Level"); }

    void OnGUI()
    {
        EditorGUILayout.Header("Level");
        difficulty = EditorGUILayout.Popup("Difficulty", difficulty, new[] { "Easy", "Normal", "Hard" });
        night = EditorGUILayout.Toggle("Night", night);
        fogEnd = EditorGUILayout.Slider("Fog distance", fogEnd, 20f, 500f);
        advanced = EditorGUILayout.Foldout(advanced, "Advanced");
        if (advanced) gravity = EditorGUILayout.FloatField("Gravity", gravity);
        EditorGUILayout.Separator();
        if (EditorGUILayout.Button("Apply")) Debug.Log("difficulty " + difficulty + ", night " + night + ", fog " + fogEnd + ", gravity " + gravity);
    }
}
}
