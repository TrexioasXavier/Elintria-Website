namespace DocCheck_Health
{
using System.Collections;
using Elintria;
using Elintria.Standard;

public class RespawnOnDeath : ElintriaScript
{
    public GameObject spawnPoint;

    public void OnDeath() { StartCoroutine(Back()); }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<ThirdPersonController>().Teleport(spawnPoint.transform.position);
        GetComponent<Health>().Revive();
    }
}
}
