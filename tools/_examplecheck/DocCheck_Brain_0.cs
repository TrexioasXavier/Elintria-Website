namespace DocCheck_Brain
{
using Elintria;
using Elintria.ML;

public class BrainInfo : ElintriaScript
{
    public string brainPath = "Assets/ML/Roller.elbrain";

    public override void Start()
    {
        Brain b = Brain.Load(brainPath);
        if (b == null) { Debug.Log("Not trained yet"); return; }
        Debug.Log(b.behaviour + ": " + b.steps + " steps, mean reward " + b.meanReward.ToString("0.00") +
                  ", sees " + b.observations + " numbers, " + b.continuous + " continuous actions");
    }
}
}
