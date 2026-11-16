namespace DocCheck_MenuItemAttribute
{
using Elintria;

public static class MenuCommands
{
    [MenuItem("Tools/Create Ground Plane")]
    static void CreateGround()
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Plane);
        g.name = "Ground";
        g.transform.localScale = new Vector3(10f, 1f, 10f);
    }
}
}
