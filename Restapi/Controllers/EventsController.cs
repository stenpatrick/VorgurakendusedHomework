using ITB2203Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _context;

        public EventsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Event>> GetTests(string? name = null)
        {
            var query = _context.Events!.AsQueryable();

            if (name != null)
                query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<TextReader> GetTest(int id)
        {
            var test = _context.Tests!.Find(id);

            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        [HttpPut("{id}")]
        public IActionResult PutTest(int id, Event @event)
        {
            var dbTest = _context.Tests!.AsNoTracking().FirstOrDefault(x => x.Id == @event.Id);
            if (id != @event.Id || dbTest == null)
            {
                return NotFound();
            }

            _context.Update(@event);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Test> PostTest(Event @event)
        {
            var dbExercise = _context.Tests!.Find(@event.Id);
            if (dbExercise == null)
            {
                _context.Add(@event);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetTest), new { Id = @event.Id }, @event);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(int id)
        {
            var test = _context.Tests!.Find(id);
            if (test == null)
            {
                return NotFound();
            }

            _context.Remove(test);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
