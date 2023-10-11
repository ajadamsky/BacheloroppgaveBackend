using System.ComponentModel.DataAnnotations;

namespace BachelorOppgaveBackend.Model
{
    public class User
    {
        public User() {}

        public User(UserRole userRole, Guid azureId, string userName, string email)
        {
            UserRole = userRole;
            AzureId = azureId;
            UserName = userName;
            Email = email;
            Created = DateTime.UtcNow;
            ProfilePicture = null;

        }
            
        public Guid Id { get; set; }
        
        [Required]
        public Guid AzureId { get; set; }
        
        [Required]
        public string? UserName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string? ProfilePicture {get; set;}
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        public Guid UserRoleId { get; set; }    
        public UserRole UserRole { get; set; }
        
    }
}

