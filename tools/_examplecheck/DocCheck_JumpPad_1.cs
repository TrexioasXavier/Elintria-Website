namespace DocCheck_JumpPad
{
using Elintria;
using Elintria.Standard;

public class CrateLauncher : ElintriaScript
{
    public GameObject crate;

    public void Fire()
    {
        JumpPad pad = GetComponent<JumpPad>();
        pad.target = GameObject.FindWithTag("Player");
        pad.Launch(crate);
    }
}
}
