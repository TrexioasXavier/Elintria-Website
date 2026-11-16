namespace DocCheck_Project
{
using Elintria;

public class WhereAmI : ElintriaScript
{
    public override void Start()
    {
        Debug.Log("Project folder: " + Project.path);
        Debug.Log("Assets folder:  " + Project.assetsPath);
        Debug.Log("Level file:     " + Project.Resolve("Assets/Levels/1.json"));
    }
}
}
