using BachelorOppgaveBackend.Controllers;
using BachelorOppgaveBackend.Model;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BachelorOppgaveBackend.Services
{
    public interface INotificationManager
    {
        public void AddNotificationToUsers(Guid? postId, Guid? commentId, string type);
        public void RemoveNotificationFromUser(Guid userid, Guid id);
    }
    public class NotificationManager : INotificationManager
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNotificationToUsers(Guid? postId, Guid? commentId, string type)
        {
            List<User> users = new List<User>();
            var post = _dbContext.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post != null)
            {
                var user = _dbContext.Users.Where(u => u.Id == post.UserId).FirstOrDefault(); // Fetch Post Owner
                if (user != null)
                {
                    users.Add(user);
                }

                var favourites = _dbContext.Favorites.Where(f => f.PostId == postId).ToList();
                foreach (var fav in favourites)
                {
                    users.Add(fav.User);
                }
            }

            var comment = _dbContext.Comments.Where(c => c.Id == commentId).FirstOrDefault();
            if (comment != null)
            {
                var user = _dbContext.Users.Where(u => u.Id == comment.UserId).FirstOrDefault(); // Fetch comment owner
                if (user != null)
                {
                    users.Add(user);
                }

            }

            

            foreach (var user in users)
            {
                if (user != null)
                {
                    var noti = new Notification(type, user, post, comment);
                    _dbContext.Notifications.Add(noti);

                }
            }
            _dbContext.SaveChanges();

        }

        public void RemoveNotificationFromUser(Guid userid, Guid id)
        {
            var noti = _dbContext.Notifications.Where(n => n.Id == id).FirstOrDefault();
            if (noti == null) { return; }


            if (userid != Guid.Empty && noti != null && noti.UserId == userid)
            {
                _dbContext.Notifications.Remove(noti);
                _dbContext.SaveChanges();
            }
        }

    }
}
