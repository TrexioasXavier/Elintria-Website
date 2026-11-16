namespace DocCheck_WaitUntil
{
using System.Collections;
using Elintria;

public class AmbushTrigger : ElintriaScript
{
    public GameObject player;
    public float radius = 8f;
    public GameObject enemies;

    public override void Start() { StartCoroutine(Ambush()); }

    IEnumerator Ambush()
    {
        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, transform.position) < radius);
        enemies.SetActive(true);
        Debug.Log("Ambush!");
    }
}
}
