namespace BOC.Models
{
    public record TransferReport
    {
        public MakeTransfer Transfer { get; init; }
        public string Status { get; init; }
    }
}
