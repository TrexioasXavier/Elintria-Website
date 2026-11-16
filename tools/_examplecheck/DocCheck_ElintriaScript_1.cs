namespace DocCheck_ElintriaScript
{
using System.Collections;
using Elintria;

public class SpikeTrap : ElintriaScript
{
    public GameObject spikes;          // child object that pops up
    public float delay = 0.4f;
    public float damage = 30f;

    bool armed = true;

    public override void OnTriggerEnter(Collider other)
    {
        if (!armed || !other.gameObject.CompareTag("Player")) return;
        StartCoroutine(Fire(other.gameObject));
    }

    IEnumerator Fire(GameObject victim)
    {
        armed = false;
        yield return new WaitForSeconds(delay);
        spikes.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        victim.SendMessage("ApplyDamage", damage);
        yield return new WaitForSeconds(1.5f);
        spikes.transform.localPosition = Vector3.zero;
        armed = true;
    }
}
}
