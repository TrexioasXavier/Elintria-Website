namespace DocCheck_QuestTracker
{
using Elintria;
using Elintria.Standard;

public class PauseOnLog : ElintriaScript
{
    public override void Update() { Time.timeScale = GetComponent<QuestTracker>().logOpen ? 0f : 1f; }
}
}
