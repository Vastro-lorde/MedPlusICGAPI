namespace MedPlusICGAPI.DTOs
{
    public record GetInvoiceRequest
    {
        public required int InvoiceNumber { get; init; }
        public required string Serie { get; init; }
    }
}
