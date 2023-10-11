using BachelorOppgaveBackend.Model;
using Microsoft.AspNetCore.Mvc;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BachelorOppgaveBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
        System.Threading.Thread.Sleep(1000);

    }

    [HttpGet]
    public IActionResult GetUsers([FromHeader] Guid userId, string? userName, Guid? userRoleId, string? orderByDesc)
    {
        var user = _context.Users.Include(r => r.UserRole).Where(u => u.Id == userId).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        if (user.UserRole.Type != "Admin")
        {
            return Unauthorized("Invalid access");
        }

        IQueryable<User> users = _context.Set<User>();

        if (userName != null)
        {
            users = users.Where(u => u.UserName.Contains(userName));
        }
        if (userRoleId != null)
        {
            users = users.Where(u => u.UserRoleId == userRoleId);
        }
        if (orderByDesc == "date")
        {
            users = users.OrderByDescending(u => u.Created);
        }

        users.Include(r => r.UserRole).Take(100).ToList();

        if (!users.Any())
        {
            return NotFound();
        }
        return Ok(users);
    }


    [HttpPost]
    public IActionResult PostUser([FromForm] Guid azureId, [FromForm] string userName, [FromForm] string userEmail, [FromHeader] string token)
    {
        var secretKey = "1234";
        if (token != secretKey)
        {
            return NotFound("Invalid token");
        }

        var userExists = _context.Users.Include(u => u.UserRole).Where(u => u.AzureId == azureId).FirstOrDefault();

        if (userExists != null)
        {
            return Ok(userExists);
        }
        var role = _context.UsersRoles.Where(r => r.Type == "User").FirstOrDefault();

        if (role == null) return NotFound("Missing Role");

        var user = new User(role, azureId, userName, userEmail);

        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok(user);
    }


    [HttpPost("id/{id}")]
    public IActionResult Put([FromHeader] Guid userId, Guid id, Guid? userRoleId, string? userName, [FromBody] string? profilePicture)
    {
        var user = _context.Users.Include(r => r.UserRole).Where(u => u.Id == userId).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        if (userId != id)
        {
            if (user.UserRole.Type != "Admin")
            {
                return Unauthorized("Invalid access");
            }
        }


        var u = _context.Users.Include(u => u.UserRole).Where(u => u.Id == id).FirstOrDefault();
        if (u == null)
        {
            return NotFound("User not found");
        }


        if (userRoleId != null)
        {
            var role = _context.UsersRoles.Where(r => r.Id == userRoleId).FirstOrDefault();
            if (role == null)
            {
                return NotFound("Role not found");
            }

            u.UserRole = role;
        }

        if (userName != null)
        {
            u.UserName = userName;
        }

        if (profilePicture != null)
        {
            Console.WriteLine("Updating profile picture");
            Console.WriteLine(profilePicture);
            u.ProfilePicture = profilePicture;
        }

        _context.Users.Update(u);
        _context.SaveChanges();
        return Ok();
    }


    [HttpDelete]
    public IActionResult Delete()
    {
        return Ok();
    }
}