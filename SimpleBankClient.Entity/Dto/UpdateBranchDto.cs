using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class UpdateBranchDto
    {
        public string? IFSC { get; set; }
        [Required]
        public string? BranchName { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? Country { get; set; }
    }

}
