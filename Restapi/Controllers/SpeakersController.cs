using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly DataContext _context;

    public SpeakersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Speaker>> GetTests(string? name = null)
    {
        var query = _context.Speakers!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetTest(int id)
    {
        var speaker = _context.Speakers!.Find(id);

        if (speaker == null)
        {
            return BadRequest();
        }
        if (speaker.Email != null && !speaker.Email.Contains("@"))
        {
            return BadRequest();
        }
        return Ok(speaker);
    }

    [HttpPut("{id}")]
    public IActionResult PutTest(int id, Speaker speaker)
    {
        var dbSpeaker = _context.Speakers!.AsNoTracking().FirstOrDefault(x => x.Id == speaker.Id);
        if (id != speaker.Id || dbSpeaker == null)
        {
            return NotFound();
        }

        _context.Update(speaker);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Test> PostTest(Speaker speaker)
    {
        if (!speaker.Email.Contains("@"))
        {
            return BadRequest("Invalid email format. Email must contain '@' symbol.");
        }

        var dbSpeaker = _context.Speakers.Find(speaker.Id);

        if (dbSpeaker != null)
        {
            return Conflict("Speaker with the same ID already exists.");
        }

        _context.Speakers.Add(speaker);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTest), new { Id = speaker.Id }, speaker);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTest(int id)
    {
        var speaker = _context.Speakers!.Find(id);
        if (speaker == null)
        {
            return NotFound();
        }

        _context.Remove(speaker);
        _context.SaveChanges();

        return NoContent();
    }
}
