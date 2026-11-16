namespace DocCheck_Frame
{
using Elintria;

public static class HitCounter
{
    public static int thisFrame, lastFrame;
    static HitCounter() { Frame.onUpdate += () => { lastFrame = thisFrame; thisFrame = 0; }; }
    public static void Hit() { thisFrame++; }
}
}
