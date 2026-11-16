namespace DocCheck_StdVars
{
using Elintria;
using Elintria.Standard;

public class Treasure : ElintriaScript
{
    public void Claim()
    {
        StdVars.Do("give:gem:2, money:100, xp:50, set:treasure_found=1", GameObject.FindWithTag("Player"));
    }
}
}
