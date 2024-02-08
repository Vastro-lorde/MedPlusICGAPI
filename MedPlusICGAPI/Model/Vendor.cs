namespace MedPlusICGAPI.Model
{
    public class Vendor
    {
        public string BankCode { get; set; } = string.Empty;
        public string AccountingCode { get; set; } = string.Empty;
        public string IBANCode { get; set; } = string.Empty;
        public int? CurrencyCode { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string BankPostalCode { get; set; } = string.Empty;
        public int? SupplierCode { get; set; }
        public string SWIFTCode { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string BankAddress { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string OrderFax { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string CommercialName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string LegalPerson { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public string BankTown { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Phone1 { get; set; } = string.Empty;
        public string Phone2 { get; set; } = string.Empty;
    }


}
