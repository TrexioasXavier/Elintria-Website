namespace DocCheck_MenuItemAttribute
{
using Elintria;

public static class OrderedItems
{
    [MenuItem("Tools/Level/Build", false, 10)] static void Build() { Debug.Log("Build"); }
    [MenuItem("Tools/Level/Validate", false, 20)] static void Validate() { Debug.Log("Validate"); }
    [MenuItem("Tools/Level/Clear", false, 100)] static void Clear() { Debug.Log("Clear"); }
}
}
