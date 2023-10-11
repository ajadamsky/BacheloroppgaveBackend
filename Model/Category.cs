using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class Category
    {
        public Category() {}

        public Category(string type, string description)
        {
            Type = type;
            Description = description;
            Created = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        
        [Required]
        public string? Type { get; set; }
        
        [Required]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
    }
}
