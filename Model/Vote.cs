using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class Vote
    {
        public Vote() {}

        public Vote(Guid userId, Guid postId, bool liked)
        {
            UserId = userId;
            PostId = postId;
            Liked = liked;
            Created = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        
        [Required]
        public bool Liked { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        
        [Required]
        public Guid UserId { get; set; }   
        public User User { get; set; }
        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
