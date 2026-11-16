namespace DocCheck_MessageType
{
using Elintria;

public static class ErrorCount
{
    public static void Show(int errors)
    {
        MessageType t = errors == 0 ? MessageType.Info : errors < 5 ? MessageType.Warning : MessageType.Error;
        EditorGUILayout.HelpBox(errors + " problems found", t);
    }
}
}
