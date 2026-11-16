namespace DocCheck_CustomYieldInstruction
{
using System;
using Elintria;

public class WaitOrTimeout : CustomYieldInstruction
{
    readonly Func<bool> done;
    readonly float until;
    public WaitOrTimeout(Func<bool> done, float timeout) { this.done = done; until = Time.time + timeout; }
    public override bool keepWaiting => !done() && Time.time < until;
    public bool TimedOut => Time.time >= until;
}
}
