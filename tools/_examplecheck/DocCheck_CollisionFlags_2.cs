namespace DocCheck_CollisionFlags
{
using Elintria;

public class FlagLogger : ElintriaScript
{
    public override void Update()
    {
        CollisionFlags f = GetComponent<CharacterController>().collisionFlags;
        if (f != CollisionFlags.None) ScreenGUI.Text(20, 20, "Touching: " + f);
    }
}
}
