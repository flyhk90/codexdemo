namespace backend_csharp.Models;

public class ScoreResponse
{
    public int Score { get; set; }
    public List<ScoreRecord> History { get; set; } = new();
}