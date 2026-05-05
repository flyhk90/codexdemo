namespace backend_csharp.Models;

public class HealthResponse
{
    public string Status { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string ServerTime { get; set; } = string.Empty;
}
