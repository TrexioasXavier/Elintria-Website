namespace DocCheck_ThrowableUser
{
using Elintria;
using Elintria.Standard;

public class GrenadeBox : Interactable
{
    public override void Interact(Interactor who)
    {
        ThrowableUser t = who.GetComponent<ThrowableUser>();
        if (t != null) { t.Give(0, 2); StdToast.Show("+2 grenades"); }
    }
}
}
