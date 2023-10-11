using BachelorOppgaveBackend.Model;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.AspNetCore.Mvc;

namespace BachelorOppgaveBackend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
            System.Threading.Thread.Sleep(1000);

        }

        [HttpGet]
        public IActionResult GetUserNotifications([FromHeader] Guid userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserId == userId).Select(n => new Notification
            {
                Id = n.Id,
                Type = n.Type,
                CommentId = n.CommentId,
                PostId = n.PostId,
                Seen = n.Seen,
                Created = n.Created
            }).ToList();

            if (notifications.Any()) { return Ok(notifications); }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Post(Guid userId, Guid? postId, Guid? commentId, string type)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            var post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
            var comment = _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
            if (user != null)
            {
                var noti = new Notification(type, user, post, comment);
                _context.Notifications.Add(noti);
                _context.SaveChanges();
                return Ok(noti);
            }
            return NotFound();
        }

        [HttpPut("seen/{id}")]
        public IActionResult SetSeen(Guid userId, Guid id)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null) { return NotFound(); }

            var noti = _context.Notifications.Where(n => n.Id == id).FirstOrDefault();
            if (noti == null) { return NotFound(); }

            noti.Seen = true;
            _context.Notifications.Update(noti);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("/{id}")]
        public IActionResult Delete([FromHeader] Guid userid, Guid id)
        {
            var noti = _context.Notifications.Where(n => n.Id == id).FirstOrDefault();
            if (noti == null) { return NotFound(); }


            if (userid != Guid.Empty && noti != null && noti.UserId == userid)
            {
                _context.Notifications.Remove(noti);
                _context.SaveChanges();
            }
            return Ok();
        }
    }
}
