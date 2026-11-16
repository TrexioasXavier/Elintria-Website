namespace DocCheck_DoubleJumpAbility
{
using Elintria;
using Elintria.Standard;

public class JumpPips : ElintriaScript
{
    public override void Update()
    {
        int left = GetComponent<DoubleJumpAbility>().jumpsLeft;
        for (int i = 0; i < left; ++i) ScreenGUI.Fill(new Rect(20f + i * 14f, 120f, 10f, 10f), Color.white);
    }
}
}
