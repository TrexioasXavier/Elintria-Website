namespace DocCheck_UIBridge
{
using Elintria;

public class DialogueChoices : ElintriaScript
{
    // Each choice: the button's name and the action it runs.
    public string[] buttons = { "choice1", "choice2", "choice3" };
    public string[] actions = { "msg:Shopkeeper.OpenShop", "scene:Scenes/Town", "quit" };

    public override void Update()
    {
        for (int i = 0; i < buttons.Length && i < actions.Length; ++i)
        {
            if (!UIRuntime.WasClicked(buttons[i])) continue;
            string error = UIBridge.Run(actions[i]);
            if (error != "") Debug.LogWarning("Choice " + i + ": " + error);
        }
    }
}
}
