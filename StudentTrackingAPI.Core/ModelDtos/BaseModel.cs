namespace StudentTrackingAPI.Core.ModelDtos
{
    public class BaseModel
    {
        public string? OperationType { get; set; }
        public string? Server_Value { get; set; }
    }
    public class Outcome
    {
        public int OutcomeId { get; set; }
        public string OutcomeDetail { get; set; }
        public string? Tokens { get; set; }
        public string? Expiration { get; set; }
        public string? UserNamee { get; set; }
        public string? DecryptedPass { get; set; }
        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public string? IpAddress { get; set; }


    }


    public class Result
    {
        public Outcome? Outcome { get; set; }
        public object? Data { get; set; }
        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public string? IpAddress { get; set; }


    }
}