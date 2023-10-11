using BachelorOppgaveBackend.Model;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
            System.Threading.Thread.Sleep(1000);

        }

        [HttpGet]
        public IActionResult GetRoles([FromHeader] Guid userId)
        {
            var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserRole.Type == "Admin")
            {
                var roles = _context.UsersRoles.ToList();
                if (roles == null)
                {
                    return NotFound();
                }
                return Ok(roles);
            }

            return Unauthorized();
        }


        [HttpPost]
        public IActionResult Post([FromHeader] Guid userId, [FromForm][Required] string roleType, [FromForm] string description, [FromForm] Guid? roleId)
        {
            var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserRole.Type == "Admin")
            {
                var role = _context.UsersRoles.Where(r => r.Id == roleId).FirstOrDefault();
                if (role == null)
                {
                    UserRole ur = new UserRole(roleType, description);
                    _context.UsersRoles.Add(ur);
                    _context.SaveChanges();
                    return Ok();
                }

                role.Type = roleType;
                role.Description = description;
                role.Created = DateTime.UtcNow;
                _context.UsersRoles.Update(role);

                _context.SaveChanges();
                return Ok();
            }

            return Unauthorized();
        }

        [HttpDelete("role/{id}")]
        public IActionResult DeleteRole([FromHeader] Guid userId, Guid id)
        {
            var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserRole.Type == "Admin")
            {
                var role = _context.UsersRoles.Where(r => r.Id == id).FirstOrDefault();
                if (role != null)
                {
                    _context.UsersRoles.Remove(role);
                    _context.SaveChanges();
                    return Ok();
                }
                return NotFound();

            }

            return Unauthorized();
        }

    }
}
