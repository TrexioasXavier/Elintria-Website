namespace DocCheck_SpriteRenderer
{
using System.Collections;
using Elintria;

public class HurtFlash : ElintriaScript
{
    public void ApplyDamage(float amount) { StartCoroutine(Flash()); }

    IEnumerator Flash()
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        int order = s.sortingOrder;
        s.sortingOrder = order + 10;     // draw on top while flashing
        s.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        s.color = Color.white;
        s.sortingOrder = order;
    }
}
}
