using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class BranchDto
    {
        [MaxLength(20)]
        public required string IFSC { get; set; }
        [MaxLength(50)]
        public string BranchName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;
    }
}
