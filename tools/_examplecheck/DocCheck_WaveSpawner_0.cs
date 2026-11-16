namespace DocCheck_WaveSpawner
{
using Elintria;
using Elintria.Standard;

public class ArenaSetup : ElintriaScript
{
    public GameObject rewardChest;

    public override void Start()
    {
        WaveSpawner w = GetComponent<WaveSpawner>();
        w.waveCount = 5;
        w.firstWaveCount = 3;
        w.countIncrease = 2;
        w.timeBetweenWaves = 8f;
        w.onAllClearedTargets.Add(rewardChest);
        w.onAllClearedMessage = "SetActive:true";
        w.Begin();
    }
}
}
