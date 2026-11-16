namespace DocCheck_EditorApplication
{
using Elintria;

public static class AutoNote
{
    static float last;

    [MenuItem("Tools/Start Reminder")]
    static void Start()
    {
        EditorApplication.update += () =>
        {
            if (Time.realtimeSinceStartup - last > 600f) { last = Time.realtimeSinceStartup; Debug.Log("Ten minutes passed: remember to save!"); }
        };
    }
}
}
