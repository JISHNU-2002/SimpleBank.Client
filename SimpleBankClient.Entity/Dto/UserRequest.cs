using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class UserRequest
    {
        [EmailAddress]
        public required string Username { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
