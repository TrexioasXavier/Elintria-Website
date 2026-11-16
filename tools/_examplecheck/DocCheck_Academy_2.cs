namespace DocCheck_Academy
{
using Elintria;
using Elintria.ML;

public class QuietTraining : ElintriaScript
{
    public override void Start()
    {
        if (Academy.Training) Audio.masterVolume = 0f;
    }
}
}
