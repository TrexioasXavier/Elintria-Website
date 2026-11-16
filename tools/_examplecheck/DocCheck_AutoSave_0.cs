namespace DocCheck_AutoSave
{
using Elintria;
using Elintria.Standard;

public class TimedSaves : ElintriaScript
{
    public override void Start() { GetComponent<AutoSave>().everyMinutes = 5f; }
}
}
