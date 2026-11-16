namespace DocCheck_WaitForSeconds
{
using System.Collections;
using Elintria;

public class Respawn : ElintriaScript
{
    public GameObject spawnPoint;
    public float delay = 3f;

    public void OnDeath() { StartCoroutine(Back()); }

    IEnumerator Back()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        transform.position = spawnPoint.transform.position;
        gameObject.SetActive(true);
    }
}
}
