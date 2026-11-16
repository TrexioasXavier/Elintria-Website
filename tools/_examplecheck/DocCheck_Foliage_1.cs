namespace DocCheck_Foliage
{
using System.Collections;
using Elintria;

public class GrassBlast : ElintriaScript
{
    public IEnumerator Blast(Vector3 at)
    {
        for (float r = 0f; r < 6f; r += Time.deltaTime * 15f)
        {
            Foliage.SetBender(7, at, r);
            yield return null;
        }
        Foliage.ClearBender(7);
    }
}
}
