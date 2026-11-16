namespace DocCheck_HitFeedback
{
using Elintria;
using Elintria.Standard;

public class HurtSound : ElintriaScript
{
    public AudioSource grunt;

    public void OnDamaged(float amount)
    {
        if (grunt != null) grunt.Play();
    }
}
}
