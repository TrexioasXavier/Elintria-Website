namespace DocCheck_CharacterStats
{
using Elintria;
using Elintria.Standard;

public class FastLevels : ElintriaScript
{
    public override void Start()
    {
        CharacterStats s = GetComponent<CharacterStats>();
        s.xpPerLevel = 50;
        s.pointsPerLevel = 2;
    }
}
}
