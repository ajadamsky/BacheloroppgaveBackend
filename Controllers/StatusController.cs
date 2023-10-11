using BachelorOppgaveBackend.Model;
using BachelorOppgaveBackend.PostgreSQL;
using BachelorOppgaveBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BachelorOppgaveBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationManager _notificationManager;


        public StatusController(ApplicationDbContext context, INotificationManager notificationManager)
        {
            _context = context;
            _notificationManager = notificationManager;
            System.Threading.Thread.Sleep(1000);

        }

        [HttpGet("post/{id}")]
        public IActionResult GetStatus([FromHeader] Guid userId, Guid id)
        {
            var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            var post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            var status = _context.Statuses.Where(s => s.Id == post.StatusId).Select(s => new Status
            {
                Id = s.Id,
                Created = DateTime.Now,
                Description = s.Description,
                Type = s.Type,
            }).FirstOrDefault();

            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }


        [HttpPost]
        public IActionResult EditStatus([FromHeader] Guid userId, [FromForm] Guid postId, [FromForm] string? type, [FromForm] string? description)
        {
            var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            var post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post == null)
            {
                return NotFound();
            }

            var status = _context.Statuses.Where(s => s.Id == post.StatusId).FirstOrDefault();
            if (status == null)
            {
                return NotFound();
            }

            if (user.UserRole.Type == "Admin")
            {
                if (!String.IsNullOrEmpty(type))
                {
                    status.Type = type;
                }

                status.Description = description;
                status.UserId = userId;
                _context.Statuses.Update(status);

                _notificationManager.AddNotificationToUsers(postId, null, $"statusChanged: {type}");

                _context.SaveChanges();
                return Ok();
            }

            return Unauthorized();

        }

    }
}