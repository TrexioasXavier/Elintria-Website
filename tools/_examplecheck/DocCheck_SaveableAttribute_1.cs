namespace DocCheck_SaveableAttribute
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class VisitedPlaces : ElintriaScript
{
    [Saveable] public List<string> visited = new List<string>();

    public void Visit(string place) { if (!visited.Contains(place)) visited.Add(place); }
}
}
