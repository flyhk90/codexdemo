namespace backend_csharp.Models;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public string Provider { get; set; } = "Sqlite";
    public string ConnectionStringName { get; set; } = "DefaultConnection";
    public bool EnsureCreated { get; set; } = true;
}
