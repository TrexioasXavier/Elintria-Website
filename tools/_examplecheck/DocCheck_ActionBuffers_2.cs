namespace DocCheck_ActionBuffers
{
using System.Collections.Generic;
using Elintria;
using Elintria.ML;

public class BrainQuery : ElintriaScript
{
    public override void Start()
    {
        Brain b = Brain.Load("Assets/ML/Roller.elbrain");
        if (b == null) return;
        var act = new ActionBuffers(b.continuous, b.branches.Length);
        b.Act(new List<float> { 1f, 0f, 2f, 0f, 0f }, act);
        Debug.Log("It would push " + act.continuous[0] + ", " + act.continuous[1]);
    }
}
}
