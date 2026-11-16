namespace DocCheck_CollapsingFloor
{
using Elintria;
using Elintria.Standard;

public class HarderFloors : ElintriaScript
{
    public void OnRespawn()
    {
        foreach (CollapsingFloor f in FindObjectsOfType<CollapsingFloor>()) f.delay = Mathf.Max(0.2f, f.delay - 0.1f);
    }
}
}
