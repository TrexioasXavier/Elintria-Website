namespace DocCheck_WaitUntil
{
using System.Collections;
using Elintria;

public class Tutorial : ElintriaScript
{
    string hint;

    public override void Start() { StartCoroutine(Steps()); }

    IEnumerator Steps()
    {
        hint = "Press W to walk";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.W));
        hint = "Press Space to jump";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        hint = null;
    }

    public override void Update()
    {
        if (hint != null) ScreenGUI.Label(new Rect(0, 40, ScreenGUI.screenWidth, 30), hint, null, Color.white, TextAnchor.MiddleCenter);
    }
}
}
