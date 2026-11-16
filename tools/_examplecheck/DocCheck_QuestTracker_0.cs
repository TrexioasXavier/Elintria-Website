namespace DocCheck_QuestTracker
{
using Elintria;
using Elintria.Standard;

public class LogOnL : ElintriaScript
{
    public override void Awake() { GetComponent<QuestTracker>().logKey = "L"; }
}
}
