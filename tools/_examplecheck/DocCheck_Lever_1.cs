namespace DocCheck_Lever
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class LeverPuzzle : ElintriaScript
{
    public List<GameObject> levers = new List<GameObject>();
    public GameObject door;
    bool solved;

    public override void Update()
    {
        if (solved || levers.Count < 3) return;
        bool a = levers[0].GetComponent<Lever>().isOn, b = levers[1].GetComponent<Lever>().isOn, c = levers[2].GetComponent<Lever>().isOn;
        if (a && !b && c) { solved = true; door.SendMessage("Open"); }
    }
}
}
