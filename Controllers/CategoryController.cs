using System.Diagnostics.Metrics;
using System.Runtime.InteropServices.ComTypes;
using BachelorOppgaveBackend.Model;
using Microsoft.AspNetCore.Mvc;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BachelorOppgaveBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
        System.Threading.Thread.Sleep(1000);

    }
    
    
    [HttpGet]
    public IActionResult Get()
    {
        var categories = _context.Categories.ToList();
        if (categories == null)
        {
            return NotFound();
        }
        return Ok(categories);
    }
    
    
    [HttpPost]
    public IActionResult Post([FromHeader] Guid userId, [FromForm] string type, [FromForm] string description)
    {

        var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
        if(user == null)
        {
            return NotFound();
        }
        if(user.UserRole.Type != "Admin") 
        {
            return Unauthorized("Invalid access");
        }

        var cat = new Category(type, description);
        _context.Add(cat);
        _context.SaveChanges();

        return Ok();
    }

    
    [HttpPut("id/{id}")]
    public IActionResult Put([FromHeader] Guid userId, Guid id, [FromForm] string? type, [FromForm] string? description)
    {
        var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
        if(user == null)
        {
            return NotFound();
        }
        if(user.UserRole.Type != "Admin") 
        {
            return Unauthorized("Invalid access");
        }

        var cat = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if(cat == null){
            return NotFound();
        }

        if(type != null){
            cat.Type = type;
        }
        if(description != null){
            cat.Description = description;
        }

        _context.Update(cat);
        _context.SaveChanges();
        return Ok();
    }

    
    [HttpDelete("id/{id}")]
    public IActionResult Delete([FromHeader] Guid userId, Guid id)
    {
        var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
        if(user == null)
        {
            return NotFound();
        }
        if(user.UserRole.Type != "Admin") 
        {
            return Unauthorized("Invalid access");
        }

        var cat = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if(cat == null){
            return NotFound();
        }

        _context.Remove(cat);
        _context.SaveChanges();
        return Ok();
    }

}