namespace DocCheck_Foliage
{
using System.Collections.Generic;
using Elintria;

public class HerdGrass : ElintriaScript
{
    public List<GameObject> animals = new List<GameObject>();

    public override void Update()
    {
        for (int i = 0; i < Foliage.MaxBenders; i++)
        {
            if (i < animals.Count && animals[i] != null) Foliage.SetBender(i, animals[i].transform.position, 1.2f);
            else Foliage.ClearBender(i);
        }
    }
}
}
