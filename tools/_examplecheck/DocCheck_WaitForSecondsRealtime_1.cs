namespace DocCheck_WaitForSecondsRealtime
{
using System.Collections;
using Elintria;

public class Toast : ElintriaScript
{
    string text;

    public void Show(string message) { StartCoroutine(Run(message)); }

    IEnumerator Run(string message)
    {
        text = message;
        yield return new WaitForSecondsRealtime(2f);
        text = null;
    }

    public override void Update()
    {
        if (text != null) ScreenGUI.Label(new Rect(20, 60, 400, 30), text);
    }
}
}
