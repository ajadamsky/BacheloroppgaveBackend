using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class Comment
    {
        public Comment() {}

        public Comment(Post post, User user, Comment? parentComment, string content)
        {
            Post = post;
            User = user;
            ParentComment = parentComment;
            Content = content;
            Created = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment {get; set;}
        public List<Comment>? ChildrenComments {get; set;}
        
    }

    public class CommentList
    {
        public CommentList(){}

        public Guid Id { get; set; }
        
        public string Content { get; set; }
        
        public DateTime Created { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment {get; set;}
        public List<Comment>? ChildrenComments {get; set;}
    }
}
