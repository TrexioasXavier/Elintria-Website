namespace DocCheck_Recipe
{
using Elintria;
using Elintria.Standard;

public class QuickCraft : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.H)) return;
        foreach (Recipe r in Recipe.Parse("medkit:1=cloth:2+herb:1"))
            if (r.Make(GetComponent<Inventory>())) StdToast.Show("Made a medkit");
    }
}
}
