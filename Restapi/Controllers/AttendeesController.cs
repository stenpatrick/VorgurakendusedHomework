using ITB2203Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public ActionResult<IEnumerable<Attendee>> GetAttendees(string? name = null, int? daysBeforeEvent = null)
        {
            var query = _context.Attendees!.AsQueryable();

            if (name != null)
            {
                query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));
            }
            if (daysBeforeEvent is not null)
            {
                query = query.Where(x=> 
                (_context!.Events!.FirstOrDefault((e) => e.Id == x.EventId)!.Date!.Value - x.RegistrationTime).TotalDays > daysBeforeEvent);
            }

            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<TextReader> GetAttendee(int id)
        {
            var attendee = _context.Attendees!.Find(id);

            if (attendee == null)
            {
                return NotFound();
            }

            if (!attendee.Email.Contains("@"))
            {
                return BadRequest();
            }


            return Ok(attendee);
        }

        [HttpPut("{id}")]
        public IActionResult PutAttendee(int id, Attendee attendee)
        {
            var dbAttendee = _context.Attendees!.AsNoTracking().FirstOrDefault(x => x.Id == attendee.Id);
            if (id != attendee.Id || dbAttendee == null)
            {
                return NotFound();
            }

            _context.Update(attendee);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Attendee> PostAttendee(Attendee attendee)
        {


            if (!attendee.Email.Contains("@"))
            {
                return BadRequest("Invalid email format. Email must contain '@' symbol.");
            }

            var existingSpeaker = _context.Speakers.FirstOrDefault(s => s.Email == attendee.Email);

            if (existingSpeaker != null)
            {
                return BadRequest("Speaker with the same email already exists.");
            }

            var dbAttendeeId = _context.Attendees.Find(attendee.Id);
            if (dbAttendeeId != null)
            {
                return Conflict("Attendee with the same ID already exists.");
            }
            var existingAttendee = _context.Attendees.FirstOrDefault(s => s.Email == attendee.Email);
            if (existingAttendee != null)
            {
                return BadRequest("Email already exists.");
            }
            var associatedEvent = _context!.Events?.FirstOrDefault(s => s.Id == attendee.EventId);
            if (associatedEvent == null)
            {
                return NotFound("Return not found.");
            }
            if (attendee.RegistrationTime > associatedEvent.Date)
            {
                return BadRequest("Registration time cannot be later than event time.");
            }
            _context.Attendees.Add(attendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendee), new { Id = attendee.Id }, attendee);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttendee(int id)
        {
            var attendee = _context.Attendees!.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            _context.Remove(attendee);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
