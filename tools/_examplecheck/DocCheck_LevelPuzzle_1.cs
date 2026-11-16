namespace DocCheck_LevelPuzzle
{
using Elintria;
using Elintria.Standard;

public class ThreeSwitches : ElintriaScript
{
    public GameObject vaultDoor;
    public GameObject[] switches;

    public override void Start()
    {
        LevelPuzzle p = gameObject.AddComponent<LevelPuzzle>();
        p.mode = 0;
        p.order = "3,1,2";
        p.door = vaultDoor;
        foreach (GameObject s in switches) p.levers.Add(s);
    }
}
}
