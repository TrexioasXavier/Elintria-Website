namespace DocCheck_Plane
{
using Elintria;

public class FinishLine : ElintriaScript
{
    public GameObject runner;
    bool wasBefore = true;

    public override void Update()
    {
        Plane line = new Plane(transform.forward, transform.position);
        bool before = !line.GetSide(runner.transform.position);
        if (wasBefore && !before) Debug.Log(runner.name + " crossed the finish line!");
        wasBefore = before;
    }
}
}
