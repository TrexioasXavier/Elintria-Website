namespace DocCheck_Agent
{
using Elintria;
using Elintria.ML;

public class BrainSwitch : ElintriaScript
{
    public GameObject agentObject;

    public override void Update()
    {
        Roller r = agentObject.GetComponent<Roller>();
        if (r == null) return;
        if (Input.GetKeyDown(KeyCode.B)) r.brain = "Assets/ML/Roller.elbrain";
        if (Input.GetKeyDown(KeyCode.H)) r.heuristicOnly = !r.heuristicOnly;
        ScreenGUI.Text(20, 20, "Mode: " + r.Mode + "  reward " + r.CumulativeReward.ToString("0.00") + "  episodes " + r.CompletedEpisodes);
    }
}
}
