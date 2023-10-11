namespace BachelorOppgaveBackend.Model
{
    public class UserRole
    {
        public UserRole() {}

        public UserRole(string type, string description)
        { 
            Type = type;
            Description = description;
            Created = DateTime.UtcNow;

        }

        
        public Guid Id { get; set; }
        
        public string? Type { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }
    }
}
