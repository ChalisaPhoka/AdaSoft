using Microsoft.AspNetCore.Mvc;
using Item1.Models;
using Item1.Data;

namespace Item1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly DBContext _context;

    public EmployeesController(ILogger<EmployeesController> logger, DBContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.employees.ToList());
    }
    [HttpGet("{id:int}")]
    public IActionResult GetByID(int id)
    {
        var exist = _context.employees.FirstOrDefault(e => e.EmployeeID == id);
        if (exist == null)
        {
            return NotFound();
        }
        return Ok(exist);
    }
    [HttpPost]
    public IActionResult Post([FromBody] Employees employees)
    {
        var exist = _context.employees.Any(e => e.EmployeeID == employees.EmployeeID);
        if (exist)
        {
            return Conflict();
        }
        var entry = _context.Add(employees);
        _context.SaveChanges();
        var newEmployee = entry.Entity;
        return CreatedAtAction(
            nameof(GetByID),
            new {id = newEmployee.EmployeeID},
            newEmployee
        );
    }
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Employees employees)
    {
        var exist = _context.employees.FirstOrDefault(e => e.EmployeeID == id);
        if (exist == null)
        {
            return NotFound();
        }
        exist.FirstName = employees.FirstName;
        exist.LastName = employees.LastName;
        exist.Age = employees.Age;
        _context.Update(exist);
        _context.SaveChanges();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var exist = _context.employees.FirstOrDefault(e => e.EmployeeID == id);
        if (exist == null)
        {
            return NotFound();
        }
        _context.employees.Remove(exist);
        _context.SaveChanges();
        return Ok();
    }
}