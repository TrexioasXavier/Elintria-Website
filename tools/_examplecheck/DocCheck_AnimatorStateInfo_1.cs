namespace DocCheck_AnimatorStateInfo
{
using Elintria;

public class AnimDebug : ElintriaScript
{
    public override void Update()
    {
        AnimatorStateInfo s = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        ScreenGUI.Text(20, 20, s.name + " " + (s.normalizedTime % 1f * 100f).ToString("0") + "%");
    }
}
}
