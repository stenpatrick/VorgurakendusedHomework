using ITB2203Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly DataContext _context;

        public AttendeesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Attendee>> GetTests(string? name = null)
        {
            var query = _context.Tests!.AsQueryable();

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
        public IActionResult PutTest(int id, Attendee attendee)
        {
            var dbTest = _context.Tests!.AsNoTracking().FirstOrDefault(x => x.Id == attendee.Id);
            if (id != attendee.Id || dbTest == null)
            {
                return NotFound();
            }

            _context.Update(attendee);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Test> PostTest(Attendee attendee)
        {
            var dbExercise = _context.Tests!.Find(attendee.Id);
            if (dbExercise == null)
            {
                _context.Add(attendee);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetTest), new { Id = attendee.Id }, attendee);
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
