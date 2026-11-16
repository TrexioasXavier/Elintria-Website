namespace DocCheck_Frame
{
using System;
using Elintria;

public class OneShotHook : ElintriaScript
{
    Action hook;

    public override void Start()
    {
        hook = () => Debug.Log("frame " + Time.frameCount);
        Frame.onUpdate += hook;
    }

    public override void OnDestroy()
    {
        Frame.onUpdate -= hook;   // always unhook what you hooked
    }
}
}
