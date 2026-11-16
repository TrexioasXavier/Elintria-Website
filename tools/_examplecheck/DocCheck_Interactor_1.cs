namespace DocCheck_Interactor
{
using Elintria;
using Elintria.Standard;

public class LookHighlight : ElintriaScript
{
    public override void Update()
    {
        Interactable it = GetComponent<Interactor>().current;
        if (it != null) Debug.DrawLine(transform.position + Vector3.up, it.Point(), Color.yellow);
    }
}
}
