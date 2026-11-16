namespace DocCheck_CharacterStats
{
using Elintria;
using Elintria.Standard;

public class LevelGate : Interactable
{
    public int needed = 5;

    public override bool CanInteract(Interactor who, out string reason)
    {
        CharacterStats s = who.GetComponent<CharacterStats>();
        reason = "Level " + needed + " needed";
        return s != null && s.level >= needed;
    }
}
}
