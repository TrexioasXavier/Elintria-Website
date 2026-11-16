namespace DocCheck_Academy
{
using Elintria;
using Elintria.ML;

public class TrainingSpeed : ElintriaScript
{
    public float respawnDelay = 3f;

    public float Delay() => Academy.Training ? 0f : respawnDelay;
}
}
