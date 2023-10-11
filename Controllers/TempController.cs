using BachelorOppgaveBackend.Model;
using Microsoft.AspNetCore.Mvc;
using BachelorOppgaveBackend.PostgreSQL;

namespace BachelorOppgaveBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class TempController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TempController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public IActionResult InitDb()
    {
        
        new ApplicationDbInitializer().Initialize(_context);
        
        return Ok("Init db");
    }

}



// Template
/*
 
 
using BachelorOppgaveBackend.Model;
using Microsoft.AspNetCore.Mvc;
using BachelorOppgaveBackend.PostgreSQL;

namespace BachelorOppgaveBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerName : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ControllerName(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
    
    
    [HttpPost]
    public IActionResult Post()
    {
        return Ok();
    }

    
    [HttpPut]
    public IActionResult Put()
    {
        return Ok();
    }

    
    [HttpDelete]
    public IActionResult Delete()
    {
        return Ok();
    }
}


*/

