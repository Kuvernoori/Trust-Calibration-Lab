using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Data;

namespace TrustCalibrationLab.Api.Controllers;

[ApiController]
[Route("api/cases")]
public sealed class CasesController(LabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List() => Ok(await db.Cases.AsNoTracking().Select(item => new
    {
        item.Id, item.Title, item.Description, item.Difficulty, item.VulnerabilityCount, item.CommentCount, HintLevel = item.HintLevel.ToString()
    }).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await db.Cases.AsNoTracking().Where(value => value.Id == id).Select(value => new
        {
            value.Id, value.Title, value.Description, value.Difficulty, value.VulnerabilityCount, value.CommentCount, HintLevel = value.HintLevel.ToString()
        }).SingleOrDefaultAsync();
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("{id:int}/files")]
    public async Task<IActionResult> Files(int id) => await db.Cases.AnyAsync(item => item.Id == id)
        ? Ok(await db.CaseFiles.AsNoTracking().Where(item => item.CaseId == id).Select(item => new { item.Id, item.CaseId, item.FilePath }).ToListAsync())
        : NotFound();

    [HttpGet("{id:int}/comments")]
    public async Task<IActionResult> Comments(int id) => await db.Cases.AnyAsync(item => item.Id == id)
        ? Ok(await db.ReviewComments.AsNoTracking().Where(item => item.CaseId == id).Select(item => new { item.Id, item.CaseId, item.FilePath, item.LineNumber, item.Text }).ToListAsync())
        : NotFound();

    [HttpGet("{id:int}/files/{fileId:int}")]
    public async Task<IActionResult> File(int id, int fileId)
    {
        var file = await db.CaseFiles.AsNoTracking().Where(item => item.CaseId == id && item.Id == fileId).Select(item => new { item.Id, item.CaseId, item.FilePath, item.Content }).SingleOrDefaultAsync();
        return file is null ? NotFound() : Ok(file);
    }
}
