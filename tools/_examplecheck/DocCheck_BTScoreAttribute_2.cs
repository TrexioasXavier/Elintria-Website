namespace DocCheck_BTScoreAttribute
{
using Elintria;
using Elintria.AI;

public class PropertyScore : ElintriaScript
{
    public int gold;
    public float Greed => Mathf.Clamp01(1f - gold / 100f);   // "score": "Greed"
}
}
