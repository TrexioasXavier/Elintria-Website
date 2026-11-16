namespace DocCheck_IComponentProvider
{
using System;
using Elintria;

public class Stats : ManagedComponent
{
    public int strength = 10;
    public int agility = 10;
}

public class StatsProvider : IComponentProvider
{
    public ManagedComponent Provide(GameObject go, Type wanted)
    {
        if (wanted != typeof(Stats)) return null;
        return go.CompareTag("Enemy") ? new Stats { strength = 12, agility = 8 } : new Stats();
    }
}
}
