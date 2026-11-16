namespace DocCheck_KeyCode
{
using System;
using Elintria;

public class Rebind : ElintriaScript
{
    public string jumpKey = "Space";
    KeyCode jump = KeyCode.Space;

    public override void Start()
    {
        if (!Enum.TryParse(jumpKey, true, out jump)) jump = KeyCode.Space;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(jump)) Debug.Log("Jump (" + jump + ")");
    }
}
}
