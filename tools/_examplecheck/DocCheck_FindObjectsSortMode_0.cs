namespace DocCheck_FindObjectsSortMode
{
using Elintria;

public class CountEnemies : ElintriaScript
{
    public override void Start()
    {
        var all = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        Debug.Log(all.Length + " rigidbodies in the scene");
    }
}
}
