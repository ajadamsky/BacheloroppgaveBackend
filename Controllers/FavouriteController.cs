using BachelorOppgaveBackend.Model;
using BachelorOppgaveBackend.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavouriteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavouriteController(ApplicationDbContext context)
        {
            _context = context;
            System.Threading.Thread.Sleep(1000);

        }

        [HttpGet]
        public IActionResult Get([Required][FromHeader] Guid userId)
        {
            var favourites = _context.Favorites.Where(f => f.UserId == userId).Select(f => new Favorite
            {
                Id = f.Id,
                PostId = f.PostId,
                Created = DateTime.Now,
            }).ToList();
            if (favourites.Any()) { return Ok(favourites); }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Post([Required][FromHeader] Guid userId, [Required][FromHeader] Guid postId)
        {
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
                if (post != null)
                {
                    // Check if already exists
                    var favourite = new Favorite(user, post);
                    if (_context.Favorites.Where(u => u.UserId == userId).Where(p => p.PostId == postId).FirstOrDefault() == null)
                    {
                        _context.Favorites.Add(favourite);
                        _context.SaveChanges();
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }


        [HttpDelete]
        public IActionResult Delete([Required][FromHeader] Guid userId, [Required][FromHeader] Guid postId)
        {
            var favourite = _context.Favorites.Where(p => p.PostId == postId).Where(u => u.UserId == userId).FirstOrDefault();
            if (favourite != null)
            {
                _context.Favorites.Remove(favourite);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
