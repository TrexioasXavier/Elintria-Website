namespace DocCheck_BTConditionAttribute
{
using Elintria;
using Elintria.AI;

public class FieldConditions : ElintriaScript
{
    public bool alarmRaised;                          // "condition": "alarmRaised"
    public bool IsNight => Time.time % 120f > 60f;    // "condition": "IsNight"
}
}
