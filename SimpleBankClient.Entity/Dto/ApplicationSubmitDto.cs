using System.ComponentModel.DataAnnotations;

namespace SimpleBankClient.Entity.Dto
{
    public class ApplicationSubmitDto
    {
        public int FormId { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
    }
}
