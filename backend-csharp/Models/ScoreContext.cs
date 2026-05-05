using Microsoft.EntityFrameworkCore;

namespace backend_csharp.Models;

public class ScoreContext : DbContext
{
    public ScoreContext(DbContextOptions<ScoreContext> options) : base(options) { }

    public DbSet<ScoreRecord> ScoreRecords { get; set; }
    public DbSet<CurrentScore> CurrentScores { get; set; }
}