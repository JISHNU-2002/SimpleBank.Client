using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankClient.Entity.Dto
{
    public class AccountTypeDto
    {
        public int TypeId { get; set; }
        [MaxLength(50)]
        public string TypeName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinBalance { get; set; }
    }
}
