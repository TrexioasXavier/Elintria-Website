namespace DocCheck_Coroutine
{
using System.Collections;
using Elintria;

public class ChargeAttack : ElintriaScript
{
    Coroutine charging;
    float charge;

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0)) charging = StartCoroutine(Charge());
        if (Input.GetMouseButtonUp(0) && charging != null)
        {
            StopCoroutine(charging);
            charging = null;
            Debug.Log("Released at " + Mathf.RoundToInt(charge * 100f) + "% charge");
            charge = 0f;
        }
    }

    IEnumerator Charge()
    {
        while (charge < 1f)
        {
            charge = Mathf.Min(1f, charge + Time.deltaTime / 2f);   // full in 2 seconds
            yield return null;
        }
    }
}
}
