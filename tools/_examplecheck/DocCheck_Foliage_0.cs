namespace DocCheck_Foliage
{
using Elintria;

public class PlayerGrass : ElintriaScript
{
    public int slot = 0;

    public override void Update() { Foliage.SetBender(slot, transform.position, 0.8f); }
    public override void OnDisable() { Foliage.ClearBender(slot); }
}
}
