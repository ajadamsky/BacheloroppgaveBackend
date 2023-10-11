using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class Favorite
    {
        public Favorite() {}

        public Favorite(User user, Post post)
        {
            User = user;
            Post = post;
            Created = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
    
}
