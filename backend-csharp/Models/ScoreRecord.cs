namespace backend_csharp.Models;

public class ScoreRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Value { get; set; }
    public int NewScore { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
}