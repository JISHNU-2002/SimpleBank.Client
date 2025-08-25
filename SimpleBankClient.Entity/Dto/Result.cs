namespace SimpleBankClient.Entity.Dto
{
    public abstract class Result
    {
        public List<Errors> Errors { get; set; } = new List<Errors>();
        public bool IsError => Errors != null && Errors.Any();
    }

    public class Result<T> : Result
    {
        public T? Response { get; set; }
        public string? WarningMessage { get; set; }
    }
}
