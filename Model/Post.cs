using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class Post
    {
        public Post() {}

        public Post(User user, Category category, Status status,string title, string description)
        {
            User = user;
            Category = category;
            Status = status;
            Title = title;
            Description = description;
            Created = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        
        [Required]
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
    }


    public class FormPost
    {
        public Guid? categoryId { get; set; }
        public string? title { get; set; } = "";
        public string? description { get; set; } = "";
    }
}
