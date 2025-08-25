using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class NewApplicationDto
    {
        public int FormId { get; set; }
        [MaxLength(50)]
        [Required]
        public string? FullName { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [Phone]
        [Required]
        public string? PhoneNumber { get; set; }
        [MaxLength(12)]
        [Required]
        public string? AadharNumber { get; set; }
        [MaxLength(10)]
        [Required]
        public string? PAN { get; set; }
        [MaxLength(100)]
        [Required]
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime DOB { get; set; } = DateTime.Now;
        public int AccountTypeId { get; set; }
        [Required]
        public string? IFSC { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;
        [Required]
        public string Status { get; set; } = "FormFilled";

        // For dropdowns
        public List<SelectListItem> AccountTypes { get; set; } = new();
        public List<SelectListItem> Branches { get; set; } = new();
    }
}
