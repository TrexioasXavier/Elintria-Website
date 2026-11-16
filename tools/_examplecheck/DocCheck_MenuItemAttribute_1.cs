namespace DocCheck_MenuItemAttribute
{
using Elintria;

public static class ValidatedItem
{
    [MenuItem("Tools/Reset Player Position")]
    static void ResetPlayer() { GameObject.FindWithTag("Player").transform.position = Vector3.zero; }

    [MenuItem("Tools/Reset Player Position", true)]
    static bool CanResetPlayer() => GameObject.FindWithTag("Player") != null;
}
}
