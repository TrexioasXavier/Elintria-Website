namespace DocCheck_EditorUtility
{
using Elintria;

public static class Done
{
    [MenuItem("Tools/Say Hello")]
    static void Hello() { EditorUtility.DisplayDialog("Hello", "Your tool ran.", "OK"); }
}
}
