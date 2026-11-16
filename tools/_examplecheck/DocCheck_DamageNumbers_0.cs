namespace DocCheck_DamageNumbers
{
using Elintria;
using Elintria.Standard;

public class AddNumbers : ElintriaScript
{
    public override void Start()
    {
        if (FindObjectOfType<DamageNumbers>() == null) gameObject.AddComponent<DamageNumbers>();
    }
}
}
