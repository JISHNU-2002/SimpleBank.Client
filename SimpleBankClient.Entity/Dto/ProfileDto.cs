using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class ProfileDto
    {
        public int FormId { get; set; }

        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? AadharNumber { get; set; }
        public string? PAN { get; set; }
        [Required]
        public string? Address { get; set; }
        public DateTime DOB { get; set; }
        public int AccountTypeId { get; set; }
        public string? AccountTypeName { get; set; }
        public string? IFSC { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string? AccountNumber { get; set; }
        public string? BranchName { get; set; }
        public decimal Balance { get; set; }
    }
}
