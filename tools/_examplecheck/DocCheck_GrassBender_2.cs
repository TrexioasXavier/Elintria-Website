namespace DocCheck_GrassBender
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class HerdBenders : ElintriaScript
{
    public List<GameObject> animals = new List<GameObject>();

    public override void Start()
    {
        for (int i = 0; i < animals.Count && i < Foliage.MaxBenders - 1; i++)
            animals[i].AddComponent<GrassBender>().slot = i + 1;   // slot 0 is the player's
    }
}
}
