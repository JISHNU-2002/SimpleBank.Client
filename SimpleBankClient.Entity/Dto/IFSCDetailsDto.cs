namespace SimpleBankClient.Entity.Dto
{
    public class IFSCDetailsDto
    {
        public string? IFSCCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? AccountTypeName { get; set; }
        public string? BranchName { get; set; }
        public decimal Balance { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
