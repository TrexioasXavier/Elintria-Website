namespace DocCheck_WaveSpawner
{
using Elintria;
using Elintria.Standard;

public class WavePlan : ElintriaScript
{
    public override void Start()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        int total = 0;
        for (int n = 1; n <= w.waveCount; n++) total += w.WaveSize(n);
        Debug.Log(w.waveCount + " waves, " + total + " enemies in all");
    }
}
}
