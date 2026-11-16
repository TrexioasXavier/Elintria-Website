namespace DocCheck_Brain
{
using Elintria;
using Elintria.ML;

public class DifficultyBrains : ElintriaScript
{
    public int difficulty = 1;   // 0 easy, 1 normal, 2 hard
    public GameObject enemy;

    public override void Start()
    {
        string[] files = { "Assets/ML/Enemy_easy.elbrain", "Assets/ML/Enemy.elbrain", "Assets/ML/Enemy_hard.elbrain" };
        Agent a = enemy.GetComponent<Agent>();
        if (a != null) a.brain = files[Mathf.Clamp(difficulty, 0, 2)];
    }
}
}
