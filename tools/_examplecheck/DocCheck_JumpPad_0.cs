namespace DocCheck_JumpPad
{
using Elintria;
using Elintria.Standard;

public class PoweredPad : ElintriaScript
{
    public void PowerOn() { GetComponent<JumpPad>().enabled = true; }
    public void PowerOff() { GetComponent<JumpPad>().enabled = false; }
}
}
