namespace DocCheck_WaitForEndOfFrame
{
using System.Collections;
using Elintria;

public class PhotoMode : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) StartCoroutine(Snap());
    }

    IEnumerator Snap()
    {
        yield return new WaitForEndOfFrame();
        GameWindow.Capture(Project.Resolve("photo.bmp"));
    }
}
}
