using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly DataContext _context;

    public EventsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents(string? name = null)
    {
        var query = _context.Events!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetEvent(int id)
    {
        var Event = _context.Events!.Find(id);

        if (Event == null)
        {
            return NotFound();
        }

        return Ok(Event);
    }

    [HttpPut("{id}")]
    public IActionResult PutEvent(int id, Event @event)
    {
        var dbEvent = _context.Events!.AsNoTracking().FirstOrDefault(x => x.Id == @event.Id);
        if (id != @event.Id || dbEvent == null)
        {
            return NotFound();
        }

        _context.Update(@event);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Event> PostEvent(Event @event)
    {
        if (@event == null)
        {
            return BadRequest(new { error = "Event object is null." });
        }
        //var dbEventSpeakerId = _context.Events.Find(@event.SpeakerId);
        //if (dbEventSpeakerId == null)
        //{
        //    return Conflict(new { error = "Event with the same ID already exists." });
        //}
        var dbEventId = _context.Events.Find(@event.Id);

        if (dbEventId != null)
        {
            return Conflict(new { error = "Event with the same ID already exists." });
        }

        _context.Events.Add(@event);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetEvent), new { Id = @event.Id }, @event);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(int id)
    {
        var @event = _context.Events!.Find(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Remove(@event);
        _context.SaveChanges();

        return NoContent();
    }
}
