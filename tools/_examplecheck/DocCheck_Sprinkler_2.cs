namespace DocCheck_Sprinkler
{
using Elintria;
using Elintria.Standard;

public class SprinklerItem : ElintriaScript
{
    public void OnUseItem(string id)
    {
        if (id != "sprinkler") return;
        GameObject s = GameObject.Create("Sprinkler");
        s.transform.position = transform.position + StdMotor.Facing(gameObject) * 1.5f;
        s.AddComponent<Sprinkler>();
        GetComponent<Inventory>().Remove("sprinkler", 1);
    }
}
}
