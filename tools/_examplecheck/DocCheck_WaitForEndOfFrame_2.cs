namespace DocCheck_WaitForEndOfFrame
{
using System.Collections;
using Elintria;

public class LazyBuilder : ElintriaScript
{
    public int blocks = 500;

    public override void Start() { StartCoroutine(Build()); }

    IEnumerator Build()
    {
        for (int i = 0; i < blocks; i++)
        {
            GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            b.transform.position = new Vector3(i % 25, 0f, i / 25);
            if (i % 50 == 49) yield return new WaitForEndOfFrame();   // 50 blocks a frame
        }
    }
}
}
