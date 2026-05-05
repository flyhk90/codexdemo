using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_csharp.Models;

namespace backend_csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly ScoreContext _context;

    public ScoreController(ScoreContext context)
    {
        _context = context;
        InitializeScore();
    }

    private void InitializeScore()
    {
        if (!_context.CurrentScores.Any())
        {
            _context.CurrentScores.Add(new CurrentScore());
            _context.SaveChanges();
        }
    }

    [HttpGet]
    public async Task<ActionResult<ScoreResponse>> GetScore()
    {
        var currentScore = await _context.CurrentScores.FirstOrDefaultAsync() ?? new CurrentScore();
        var history = await _context.ScoreRecords.OrderByDescending(r => r.Time).Take(20).ToListAsync();

        return Ok(new ScoreResponse
        {
            Score = currentScore.Value,
            History = history
        });
    }

    [HttpPost("add")]
    public async Task<ActionResult<ScoreResponse>> AddScore([FromBody] ScoreUpdateRequest request)
    {
        var currentScore = await _context.CurrentScores.FirstOrDefaultAsync() ?? new CurrentScore();
        currentScore.Value += request.Value;

        var record = new ScoreRecord
        {
            Value = request.Value,
            NewScore = currentScore.Value
        };

        _context.ScoreRecords.Add(record);
        await _context.SaveChangesAsync();

        var history = await _context.ScoreRecords.OrderByDescending(r => r.Time).Take(20).ToListAsync();

        return Ok(new ScoreResponse
        {
            Score = currentScore.Value,
            History = history
        });
    }

    [HttpPost("subtract")]
    public async Task<ActionResult<ScoreResponse>> SubtractScore([FromBody] ScoreUpdateRequest request)
    {
        var currentScore = await _context.CurrentScores.FirstOrDefaultAsync() ?? new CurrentScore();
        currentScore.Value -= request.Value;

        var record = new ScoreRecord
        {
            Value = -request.Value,
            NewScore = currentScore.Value
        };

        _context.ScoreRecords.Add(record);
        await _context.SaveChangesAsync();

        var history = await _context.ScoreRecords.OrderByDescending(r => r.Time).Take(20).ToListAsync();

        return Ok(new ScoreResponse
        {
            Score = currentScore.Value,
            History = history
        });
    }

    [HttpPost("reset")]
    public async Task<ActionResult<ScoreResponse>> ResetScore()
    {
        var currentScore = await _context.CurrentScores.FirstOrDefaultAsync();
        if (currentScore != null)
        {
            currentScore.Value = 0;
        }
        else
        {
            currentScore = new CurrentScore();
            _context.CurrentScores.Add(currentScore);
        }

        _context.ScoreRecords.RemoveRange(_context.ScoreRecords);
        await _context.SaveChangesAsync();

        return Ok(new ScoreResponse
        {
            Score = 0,
            History = new List<ScoreRecord>()
        });
    }

    [HttpDelete("history")]
    public async Task<ActionResult<ScoreResponse>> ClearHistory()
    {
        _context.ScoreRecords.RemoveRange(_context.ScoreRecords);
        await _context.SaveChangesAsync();

        var currentScore = await _context.CurrentScores.FirstOrDefaultAsync() ?? new CurrentScore();

        return Ok(new ScoreResponse
        {
            Score = currentScore.Value,
            History = new List<ScoreRecord>()
        });
    }
}