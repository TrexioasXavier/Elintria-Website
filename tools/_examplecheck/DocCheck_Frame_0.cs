namespace DocCheck_Frame
{
using Elintria;

public static class GameClock
{
    public static float minutesOfDay = 8f * 60f;
    public static float speed = 10f;   // game minutes per real second
    static bool hooked;

    public static void Begin()
    {
        if (hooked) return;
        hooked = true;
        Frame.onUpdate += () => minutesOfDay = (minutesOfDay + speed * Time.deltaTime) % (24f * 60f);
    }
}

public class ClockStarter : ElintriaScript
{
    public override void Start() { GameClock.Begin(); }
}
}
