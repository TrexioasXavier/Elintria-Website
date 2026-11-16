namespace DocCheck_ElintriaScript
{
using Elintria;

public class PlayerStats : ElintriaScript
{
    public float maxStamina = 100f;
    public float stamina;

    Rigidbody body;
    AudioSource audioSource;

    public override void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = gameObject.audioSource;   // adds one if missing
        stamina = maxStamina;
    }

    public override void Update()
    {
        bool running = Input.GetKey(KeyCode.Shift) && body != null && body.velocity.magnitude > 0.1f;
        stamina = Mathf.Clamp(stamina + (running ? -20f : 10f) * Time.deltaTime, 0f, maxStamina);
        if (stamina <= 0f && !audioSource.isPlaying)
            audioSource.PlayOneShot("Audio/out_of_breath.wav");
    }
}
}
