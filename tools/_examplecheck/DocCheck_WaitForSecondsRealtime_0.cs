namespace DocCheck_WaitForSecondsRealtime
{
using System.Collections;
using Elintria;

public class IntroText : ElintriaScript
{
    string line;

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        line = "Chapter One";
        yield return new WaitForSecondsRealtime(2f);
        line = "The Old Mill";
        yield return new WaitForSecondsRealtime(2f);
        line = null;
    }

    public override void Update()
    {
        if (line != null) ScreenGUI.Label(new Rect(0, 80, ScreenGUI.screenWidth, 40), line, null, Color.white, TextAnchor.MiddleCenter);
    }
}
}
