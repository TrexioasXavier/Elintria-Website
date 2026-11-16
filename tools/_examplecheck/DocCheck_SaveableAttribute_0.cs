namespace DocCheck_SaveableAttribute
{
using Elintria;
using Elintria.Standard;

public class Shrine : ElintriaScript
{
    [Saveable] public int prayers;
    [Saveable] public bool blessed;

    public void Pray() { ++prayers; if (prayers >= 3) blessed = true; }
}
}
