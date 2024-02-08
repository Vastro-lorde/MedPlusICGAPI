using MedPlusICGAPI.Model;

namespace MedPlusICGAPI.Repo
{
    public interface IInvoiceRepo
    {
        Task<Invoice> InvoiceByInvoiceNumberNSerie(int invoiceNumber, string serialNumber);
    }
}