using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class AddBranchDto
    {
        [Required]
        public required string BranchName { get; set; }
        [Required]
        public required string State { get; set; }
        [Required]
        public required string Country { get; set; }
    }
}
