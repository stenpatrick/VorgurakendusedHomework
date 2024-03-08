using Microsoft.AspNetCore.Mvc;
using teamtrack_api.Model;
using System.Linq;

[ApiController] [Route("api/[controller]")] public class PeopleController : ControllerBase
{
private readonly DataContext _context;

    public PeopleController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetPeople()
    {
        var people = _context.PeopleList.ToList();
        return Ok(people);
    }

    [HttpPost]
    public IActionResult Create([FromBody] People person)
    {
        if (_context.PeopleList.Any(p => p.Id == person.Id))
        {
            return Conflict("Person with this Id already exists.");
        }

        _context.PeopleList.Add(person);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPeople), new { id = person.Id }, person);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] People updatedPerson)
    {
        if (id != updatedPerson.Id || !_context.PeopleList.Any(p => p.Id == id))
        {
            return NotFound("Person not found.");
        }

        _context.Update(updatedPerson);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var personToDelete = _context.PeopleList.FirstOrDefault(p => p.Id == id);

        if (personToDelete == null)
        {
            return NotFound("Person not found.");
        }

        _context.PeopleList.Remove(personToDelete);
        _context.SaveChanges();

        return NoContent();
    }
}
