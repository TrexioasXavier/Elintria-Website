namespace DocCheck_QuestTracker
{
using Elintria;
using Elintria.Standard;

public class NoMarkers : ElintriaScript
{
    public bool hard;

    public override void Start() { GetComponent<QuestTracker>().showMarker = !hard; }
}
}
