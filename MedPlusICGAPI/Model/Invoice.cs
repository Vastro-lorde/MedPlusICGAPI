using MedPlusICGAPI.Model;
using Microsoft.Data.SqlClient;

namespace MedPlusICGAPI.Model
{

    public class Invoice
    {
        public int ClientCode { get; set; }
        public int? CurrencyCode { get; set; }
        public int SupplierCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime Hour { get; set; }
        public int InvoiceNumber { get; set; }
        public string SerialNumber { get; set; }
        public string YourInvoice { get; set; }
        public int DocumentType { get; set; }
        public double GrossTotal { get; set; }
        public double NetTotal { get; set; }
        public List<InvoiceItem>? InvoiceItems { get; set; }
        public Vendor? vendor { get; set; }
    }


    public class InvoiceItem
    {
        public string SerialNumber { get; set; }
        public int InvoiceNumber { get; set; }
        public int LineNumber { get; set; }
        public int ArticleCode { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public double Unit1 { get; set; }
        public double Unit2 { get; set; }
        public double Unit3 { get; set; }
        public double Unit4 { get; set; }
        public double TotalUnits { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public short TaxType { get; set; }
        public string WarehouseCode { get; set; }
        public double ExpenseAmount { get; set; }
        public double ExpandedUnits { get; set; }
        public double ExpandedTotal { get; set; }
        public int ClientCode { get; set; }
    }

}
