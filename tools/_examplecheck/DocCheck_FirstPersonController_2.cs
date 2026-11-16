namespace DocCheck_FirstPersonController
{
using Elintria;
using Elintria.Standard;

public class SpawnPlayer : ElintriaScript
{
    public override void Start()
    {
        GameObject p = GameObject.Create("Player");
        p.tag = "Player";
        p.transform.position = transform.position;
        CharacterController cc = p.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.radius = 0.35f;
        p.AddComponent<Health>();
        p.AddComponent<Inventory>();
        p.AddComponent<Interactor>();
        p.AddComponent<PlayerRespawn>();
        p.AddComponent<FirstPersonController>();
    }
}
}
