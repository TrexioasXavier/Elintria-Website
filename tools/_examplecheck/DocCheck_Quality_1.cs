namespace DocCheck_Quality
{
using Elintria;

public class AutoQuality : ElintriaScript
{
    float slowFor;

    public override void Update()
    {
        if (Time.deltaTime > 1f / 40f) slowFor += Time.deltaTime; else slowFor = 0f;
        if (slowFor > 3f && Quality.level > 0)
        {
            Quality.DecreaseLevel();
            Debug.Log("Dropped to " + Quality.levelName);
            slowFor = 0f;
        }
    }
}
}
