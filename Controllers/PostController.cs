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
public class PostController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PostController(ApplicationDbContext context)
    {
        _context = context;
        System.Threading.Thread.Sleep(1000);

    }


    [HttpGet]
    public IActionResult GetPosts([FromHeader] Guid? userId, string? query, string? orderByDesc, Guid? queryCategory, string? queryStatus)
    {
        IQueryable<Post> posts = _context.Set<Post>();

        if (query != null)
        {
            posts = posts.Where(p => EF.Functions.ILike(p.Title, $"%{query}%") || EF.Functions.ILike(p.User.UserName, $"%{query}%"));
        }

        if (queryCategory != null)
        {
            posts = posts.Where(c => c.Category.Id == queryCategory);
        }

        if (queryStatus != null)
        {
            posts = posts.Where(c => c.Status.Type == queryStatus);
        }

        var res = posts.Select(p => new
        {
            p.Id,
            p.Title,
            p.Description,
            p.Created,
            up_votes = _context.Votes.Where(v => v.Liked == true).Count(v => p.Id == v.PostId),
            down_votes = _context.Votes.Where(v => v.Liked == false).Count(v => p.Id == v.PostId),
            user = new { p.User.UserName, p.User.Email },
            category = new { p.CategoryId, p.Category.Type },
            status = new { p.StatusId, p.Status.Type },
            favourite = _context.Favorites.Where(s => p.Id == s.PostId).Where(s => userId == s.UserId).FirstOrDefault() == null ? false : true,
            liked = _context.Votes.Where(v => p.Id == v.PostId).Where(v => userId == v.UserId).Select(v => new { v.Liked }).FirstOrDefault() ?? null,
            comments = _context.Comments.Count(c => p.Id == c.PostId)
        });

        if (orderByDesc != null)
        {
            if (orderByDesc == "date")
            {
                res = res.OrderByDescending(c => c.Created);
            }
            else if (orderByDesc == "votes")
            {
                res = res.OrderByDescending(v => v.up_votes);

            }
            else if (orderByDesc == "comments")
            {
                res = res.OrderByDescending(c => c.comments);
            }
        }

        return Ok(res.ToList());
    }



    [HttpGet("id/{id}")]
    public IActionResult GetPostById([FromHeader] Guid userId, Guid id)
    {
        var posts = _context.Posts
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Description,
                p.Created,
                up_votes = _context.Votes.Where(v => v.Liked == true).Count(v => p.Id == v.PostId),
                down_votes = _context.Votes.Where(v => v.Liked == false).Count(v => p.Id == v.PostId),
                user = new { p.User.UserName, p.User.Email },
                category = new { p.CategoryId, p.Category.Type },
                status = new { p.StatusId, p.Status.Type },
                favourite = _context.Favorites.Where(s => p.Id == s.PostId).Where(s => userId == s.UserId).FirstOrDefault() == null ? false : true,
                liked = _context.Votes.Where(v => p.Id == v.PostId).Where(v => userId == v.UserId).Select(v => new { v.Liked }).FirstOrDefault() ?? null,
                comments = _context.Comments.Count(c => p.Id == c.PostId)
            })
            .Where(t => t.Id == id)
            .FirstOrDefault();

        if (posts == null)
        {
            return NotFound();
        }
        return Ok(posts);
    }


    [HttpPost]
    public IActionResult AddPost([FromHeader] Guid userId, [FromForm] FormPost post)
    {
        var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
        if (user == null)
        {
            return NotFound("Invalid user");
        }

        var category = _context.Categories.Where(c => c.Id == post.categoryId).FirstOrDefault();
        if (category == null)
        {
            return NotFound("Invaild category");
        }

        var s = new Status(null, "Venter", "Venter på svar");
        var p = new Post(user, category, s, post.title ?? "", post.description ?? "");
        _context.Posts.Add(p);
        _context.SaveChanges();
        return Ok();
    }


    [HttpPut("id/{id}")]
    public IActionResult EditPost([FromHeader] Guid userId, Guid id, [FromForm] string? title, [FromForm] string? description)
    {
        var user = _context.Users.Include(u => u.UserRole).Where(u => u.Id == userId).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }

        var post = _context.Posts.Include(u => u.User).Where(p => p.Id == id).FirstOrDefault();
        if (post == null)
        {
            return NotFound();
        }

        if (title != null)
        {
            post.Title = title;
        }

        if (description != null)
        {
            post.Description = description;
        }

        if (user.Id == post.UserId || user.UserRole.Type == "Admin")
        {
            _context.SaveChanges();
            return Ok();
        }

        return NotFound("Invalid access");
    }



    [HttpDelete("id/{id}")]
    public IActionResult DeletePost([FromHeader] Guid userId, Guid id)
    {
        var user = _context.Users.Where(u => u.Id == userId).Include(u => u.UserRole).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }

        var post = _context.Posts.Where(p => p.Id == id).Include(p => p.User).FirstOrDefault();
        if (post == null)
        {
            return NotFound();
        }

        if (user.Id == post.UserId || user.UserRole.Type == "Admin")
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return Ok();
        }

        return NotFound("Invalid access");
    }
}