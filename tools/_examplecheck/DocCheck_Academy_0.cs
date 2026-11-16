namespace DocCheck_Academy
{
using Elintria;
using Elintria.ML;

public class Intro : ElintriaScript
{
    public GameObject introCutscene;

    public override void Start()
    {
        if (Academy.Training && introCutscene != null) introCutscene.SetActive(false);
    }
}
}
