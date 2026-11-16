namespace DocCheck_SplineFollower
{
using System.Collections;
using Elintria;
using Elintria.Standard;

public class TrainStops : ElintriaScript
{
    public float[] stations = { 0f, 120f, 260f };

    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        int next = 1;
        while (true)
        {
            f.Play();
            yield return new WaitUntil(() => Mathf.Abs(f.distance - stations[next]) < 1f);
            f.Stop();
            yield return new WaitForSeconds(5f);
            next = (next + 1) % stations.Length;
        }
    }
}
}
