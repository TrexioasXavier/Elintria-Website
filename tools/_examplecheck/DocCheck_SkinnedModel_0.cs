namespace DocCheck_SkinnedModel
{
using Elintria;

public class SimpleAnimPlayer : ElintriaScript
{
    SkinnedModel model;

    public override void Start()
    {
        model = SkinnedModel.Get(gameObject);
        Debug.Log("Clips: " + string.Join(", ", model.clips));
        model.Play("idle");
    }

    public override void Update()
    {
        bool moving = Input.GetAxisVertical() != 0f;
        string want = moving ? (Input.GetKey(KeyCode.Shift) ? "run" : "walk") : "idle";
        if (model.clip != want) model.Play(want, 1f, true, 0.2f);
        if (Input.GetKeyDown(KeyCode.Space)) model.Play("jump", 1f, false, 0.1f);
    }
}
}
