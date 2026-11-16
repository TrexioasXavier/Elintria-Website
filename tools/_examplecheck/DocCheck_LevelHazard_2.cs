namespace DocCheck_LevelHazard
{
using Elintria;
using Elintria.Standard;

public class CrusherSetup : ElintriaScript
{
    public GameObject crusherHead;

    public override void Start()
    {
        LevelHazard h = gameObject.AddComponent<LevelHazard>();
        h.kind = 0;                       // moves up and down like spikes, hits once a cycle
        h.damage = 60f;
        h.size = new Vector3(2f, 2f, 2f);
        h.onTime = 0.6f;
        h.offTime = 2.4f;
        h.movingPart = crusherHead;
        h.riseHeight = -1.5f;             // comes down instead of up
        h.requiredTag = "Player";
    }
}
}
