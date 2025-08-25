using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class UserResponse
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(20)]
        public required string AccountNumber { get; set; }
        public int FormId { get; set; }


        // Roles assigned to the user
        public ICollection<Roles> roles { get; set; } = new List<Roles>();
    }
    public class Roles
    {
        public required string RoleName { get; set; }
    }
}
