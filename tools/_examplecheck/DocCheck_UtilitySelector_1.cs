namespace DocCheck_UtilitySelector
{
using Elintria;
using Elintria.AI;

public class Villager : ElintriaScript
{
    public float hunger, fatigue;

    public override void Update()
    {
        hunger = Mathf.Clamp01(hunger + 0.03f * Time.deltaTime);
        fatigue = Mathf.Clamp01(fatigue + 0.015f * Time.deltaTime);
        this.RunBehaviourTree("Assets/AI/Villager.elbt");
    }

    [BTScore] float Hunger() => hunger;
    [BTScore] float Fatigue() => fatigue;

    void Eat() { hunger = 0f; }
    void Sleep() { fatigue = 0f; }
    void Work() { }
    void Wander() { transform.Rotate(0f, 20f * Time.deltaTime, 0f); }
}
}
