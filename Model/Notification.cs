using System.ComponentModel.DataAnnotations;


namespace BachelorOppgaveBackend.Model;

public class Notification
{
    public Notification(){}

    public Notification(string type, User user, Post? post, Comment? comment)
    {
        Type = type;
        User = user;
        Post = post ?? null;
        Comment = comment ?? null;
        Seen = false;
        Created = DateTime.UtcNow;
    }
    
    public Guid Id { get; set; }
    
    [Required]
    public string Type { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Created { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid? PostId { get; set; }
    public Post? Post { get; set; }

    public Guid? CommentId { get; set; }
    public Comment? Comment { get; set; }

    public bool Seen { get; set; }
}