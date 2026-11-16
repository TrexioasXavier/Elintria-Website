namespace DocCheck_BTScoreAttribute
{
using Elintria;
using Elintria.AI;

public class SurvivorScores : ElintriaScript
{
    public float food = 0.6f;
    public float health = 1f;

    [BTScore] float Hunger() => 1f - food;
    [BTScore] float Danger() => 1f - health;
}
}
