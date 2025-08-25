using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class RoleDto
    {
        [MaxLength(100)]
        public required string RoleName { get; set; }
    }
}
