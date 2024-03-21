using Microsoft.AspNetCore.Mvc;
using teamtrack_api.Model;
using System.Linq;

namespace teamtrack_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventPeopleController : ControllerBase
    {
        private readonly DataContext _context;

        public EventPeopleController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEventPeople()
        {
            return Ok(_context.EventPeopleList);
        }

        [HttpPost]
        public IActionResult Create([FromBody] EventPeople e)
        {
            if (e == null)
            {
                return BadRequest("EventPeople object is null");
            }

            var dbEventPeople = _context.EventPeopleList.FirstOrDefault(ev => ev.Id == e.Id);
            if (dbEventPeople != null)
            {
                return Conflict("EventPeople already exists");
            }

            _context.EventPeopleList.Add(e);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEventPeople), new { id = e.Id }, e);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EventPeople e)
        {
            if (e == null || id != e.Id)
            {
                return BadRequest("EventPeople object is null or ID does not match");
            }

            var dbEventPeople = _context.EventPeopleList.FirstOrDefault(ev => ev.Id == id);
            if (dbEventPeople == null)
            {
                return NotFound("EventPeople not found");
            }

            dbEventPeople.EventId = e.EventId;
            dbEventPeople.PersonId = e.PersonId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
        var eventPeopleToDelete = _context.EventPeopleList?.Find(id);
        if (eventPeopleToDelete == null) return NotFound();
        _context.EventPeopleList.Remove(eventPeopleToDelete);
        _context.SaveChanges();
        return NoContent();
        }
    }
}
