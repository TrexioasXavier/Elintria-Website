namespace DocCheck_QuestLog
{
using Elintria;
using Elintria.Standard;

public class QuestFanfare : ElintriaScript
{
    public AudioSource fanfare;

    public override void Start() { QuestLog.Completed += q => { if (fanfare != null) fanfare.Play(); }; }
}
}
