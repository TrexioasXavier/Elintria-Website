namespace DocCheck_Cloth
{
using Elintria;
using Elintria.Standard;

public class TableclothTrick : ElintriaScript
{
    public GameObject hand;   // move this object to drag a corner

    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.horizontal = true;
        c.pinMode = "None";
        c.pins.Add(hand);
        c.friction = 0.6f;
    }

    public override void Update()
    {
        if (GetComponent<Cloth>().lowestY < -5f) Debug.Log("The tablecloth fell on the floor");
    }
}
}
