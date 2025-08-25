using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class RegisterRequest
    {
        [EmailAddress]
        public required string Username { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
