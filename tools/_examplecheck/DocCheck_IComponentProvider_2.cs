namespace DocCheck_IComponentProvider
{
using System;
using Elintria;

public class Dialogue : ManagedComponent { public string[] lines = new string[0]; }

public class DialogueProvider : IComponentProvider
{
    public ManagedComponent Provide(GameObject go, Type wanted)
    {
        if (wanted != typeof(Dialogue)) return null;
        string[] lines = Content.ReadAllLines("Assets/Dialogue/" + go.name + ".txt");
        return lines == null ? null : new Dialogue { lines = lines };
    }
}
}
