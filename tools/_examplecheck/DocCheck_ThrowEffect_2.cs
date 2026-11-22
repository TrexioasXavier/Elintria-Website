namespace DocCheck_ThrowEffect
{
using Elintria;
using Elintria.Standard;

public class SmokeCheck : ElintriaScript
{
    public GameObject guard;

    public override void Update()
    {
        if (ThrowEffect.SmokeBlocks(guard.transform.position + Vector3.up * 1.5f, transform.position + Vector3.up * 1.5f))
            ScreenGUI.Text(20f, 160f, "Hidden by smoke", null, Color.white);
    }
}
}
