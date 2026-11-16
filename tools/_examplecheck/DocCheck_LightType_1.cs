namespace DocCheck_LightType
{
using Elintria;

public class FindSun : ElintriaScript
{
    public override void Start()
    {
        foreach (Light l in FindObjectsOfType<Light>())
            if (l.type == LightType.Directional) { Debug.Log("The sun is " + l.gameObject.name); break; }
    }
}
}
