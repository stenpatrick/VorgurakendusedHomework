using ITB2203Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var test = _context.Tests!.Find(id);

            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        [HttpPut("{id}")]
        public IActionResult PutTest(int id, Speaker speaker)
        {
            var dbTest = _context.Tests!.AsNoTracking().FirstOrDefault(x => x.Id == speaker.Id);
            if (id != speaker.Id || dbTest == null)
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
            var dbExercise = _context.Tests!.Find(speaker.Id);
            if (dbExercise == null)
            {
                _context.Add(speaker);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetTest), new { Id = speaker.Id }, speaker);
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
