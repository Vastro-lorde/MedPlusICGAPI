using MedPlusICGAPI.Model;
using MedPlusICGAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace MedPlusICGAPI.Repo
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private IConfiguration _configuration;
        private static string? _connectionString;
        public InvoiceRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Default");
        }


        public async Task<Invoice> InvoiceByInvoiceNumberNSerie(int invoiceNumber, string serialNumber)
        {
            Invoice invoice = new();
            
            try
            {
                using SqlConnection connection = new(_connectionString);
                await connection.OpenAsync();

                string invoiceQuery = $"select * from ALBCOMPRACAB where NUMSERIE = '{serialNumber}' and NUMALBARAN = '{invoiceNumber}';";
                string itemsQuery = $"select * from ALBCOMPRALIN where NUMSERIE = '{serialNumber}' and NUMALBARAN = '{invoiceNumber}';";

                using (SqlCommand command = new(invoiceQuery, connection))
                {
                    using SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            invoice.ClientCode = reader.GetInt32("CODCLIENTE");
                            invoice.CurrencyCode = reader.GetInt32("CODMONEDA");
                            invoice.SupplierCode = reader.GetInt32("CODPROVEEDOR");
                            invoice.InvoiceDate = reader.GetDateTime("FECHAALBARAN");
                            invoice.CreationDate = reader.GetDateTime("FECHACREACION");
                            invoice.EntryDate = reader.GetDateTime("FECHAENTRADA");
                            invoice.ModifiedDate = reader.GetDateTime("FECHAMODIFICADO");
                            invoice.Hour = reader.GetDateTime("HORA");
                            invoice.InvoiceNumber = reader.GetInt32("NUMALBARAN");
                            invoice.SerialNumber = reader.GetString("NUMSERIE");
                            invoice.YourInvoice = reader.GetString("SUALBARAN");
                            invoice.DocumentType = reader.GetInt32("TIPODOC");
                            invoice.GrossTotal = reader.GetDouble("TOTALBRUTO");
                            invoice.NetTotal = reader.GetDouble("TOTALNETO");
                            invoice.InvoiceItems = new List<InvoiceItem>();

                        }
                    }else
                    {
                        reader.Dispose();
                        throw new NoDataFoundException("Invoice Not Found");
                    }
                    
                }

                // Assuming you have already retrieved the supplier code from the invoice

                // Now let's retrieve vendor details based on the supplier code from the invoice
                // Create a parameterized query with only the necessary columns
                string vendorQuery = @"
                                        SELECT 
                                            APTOCORREOS, CODBANCO, CODCONTABLE, CODIGOIBAN, CODMONEDA, 
                                            CODPAIS, CODPOSTAL, CODPOSTALBANCO, CODPROVEEDOR, CODSWIFT, 
                                            DIRECCION1, DIRECCION2, DIRECCIONBANCO, E_MAIL, FAXPEDIDOS, 
                                            FECHAMODIFICADO, MOBIL, NOMBREBANCO, NOMCOMERCIAL, NOMPROVEEDOR, 
                                            NUMCUENTA, PAIS, PERSONACONTACTO, PERSONAJURIDICA, POBLACION, 
                                            POBLACIONBANCO, PROVINCIA, TELEFONO1, TELEFONO2
                                        FROM PROVEEDORES 
                                        WHERE CODPROVEEDOR = @SupplierCode";


                // Use parameterized query to prevent SQL injection and improve performance
                using (SqlCommand vendorCommand = new SqlCommand(vendorQuery, connection))
                {
                    // Add parameter for SupplierCode
                    vendorCommand.Parameters.AddWithValue("@SupplierCode", invoice.SupplierCode);

                     Vendor vendor = new() { };
                    using SqlDataReader vendorReader = await vendorCommand.ExecuteReaderAsync();
                    while (vendorReader.Read())
                    {
                        vendor.BankCode = GetStringOrNull(vendorReader, "CODBANCO");
                        vendor.AccountingCode = GetStringOrNull(vendorReader, "CODCONTABLE");
                        vendor.IBANCode = GetStringOrNull(vendorReader, "CODIGOIBAN");
                        vendor.CountryCode = GetStringOrNull(vendorReader, "CODPAIS");
                        vendor.PostalCode = GetStringOrNull(vendorReader, "CODPOSTAL");
                        vendor.BankPostalCode = GetStringOrNull(vendorReader, "CODPOSTALBANCO");
                        vendor.SupplierCode = GetIntOrDefault(vendorReader, "CODPROVEEDOR");
                        vendor.CurrencyCode = GetIntOrDefault(vendorReader, "CODMONEDA");
                        vendor.SWIFTCode = GetStringOrNull(vendorReader, "CODSWIFT");
                        vendor.Address1 = GetStringOrNull(vendorReader, "DIRECCION1");
                        vendor.Address2 = GetStringOrNull(vendorReader, "DIRECCION2");
                        vendor.BankAddress = GetStringOrNull(vendorReader, "DIRECCIONBANCO");
                        vendor.Email = GetStringOrNull(vendorReader, "E_MAIL");
                        vendor.OrderFax = GetStringOrNull(vendorReader, "FAXPEDIDOS");
                        vendor.ModifiedDate = vendorReader.GetDateTime(vendorReader.GetOrdinal("FECHAMODIFICADO"));
                        vendor.Mobile = GetStringOrNull(vendorReader, "MOBIL");
                        vendor.BankName = GetStringOrNull(vendorReader, "NOMBREBANCO");
                        vendor.CommercialName = GetStringOrNull(vendorReader, "NOMCOMERCIAL");
                        vendor.SupplierName = GetStringOrNull(vendorReader, "NOMPROVEEDOR");
                        vendor.AccountNumber = GetStringOrNull(vendorReader, "NUMCUENTA");
                        vendor.Country = GetStringOrNull(vendorReader, "PAIS");
                        vendor.ContactPerson = GetStringOrNull(vendorReader, "PERSONACONTACTO");
                        vendor.LegalPerson = GetStringOrNull(vendorReader, "PERSONAJURIDICA");
                        vendor.Town = GetStringOrNull(vendorReader, "POBLACION");
                        vendor.BankTown = GetStringOrNull(vendorReader, "POBLACIONBANCO");
                        vendor.Province = GetStringOrNull(vendorReader, "PROVINCIA");
                        vendor.Phone1 = GetStringOrNull(vendorReader, "TELEFONO1");
                        vendor.Phone2 = GetStringOrNull(vendorReader, "TELEFONO2");
                    }
                    invoice.vendor = vendor;

                }


                // Fetch invoice items
                using (SqlCommand command = new SqlCommand(itemsQuery, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var item = new InvoiceItem()
                            {
                                SerialNumber = reader.GetString("NUMSERIE"),
                                InvoiceNumber = reader.GetInt32("NUMALBARAN"),
                                LineNumber = reader.GetInt32("NUMLIN"),
                                ArticleCode = reader.GetInt32("CODARTICULO"),
                                Reference = reader.GetString("REFERENCIA"),
                                Description = reader.GetString("DESCRIPCION"),
                                Unit1 = reader.GetDouble("UNID1"),
                                Unit2 = reader.GetDouble("UNID2"),
                                Unit3 = reader.GetDouble("UNID3"),
                                Unit4 = reader.GetDouble("UNID4"),
                                TotalUnits = reader.GetDouble("UNIDADESTOTAL"),
                                Price = reader.GetDouble("PRECIO"),
                                Total = reader.GetDouble("TOTAL"),
                                TaxType = reader.GetInt16("TIPOIMPUESTO"),
                                WarehouseCode = reader.GetString("CODALMACEN"),
                                ExpenseAmount = reader.GetDouble("IMPORTEGASTOS"),
                                ExpandedUnits = reader.GetDouble("UDSEXPANSION"),
                                ExpandedTotal = reader.GetDouble("TOTALEXPANSION"),
                                ClientCode = reader.GetInt32("CODCLIENTE")
                            };


                            invoice.InvoiceItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return invoice;
        }

        private static string? GetStringOrNull(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static double? GetDoubleOrNull(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (double?)null : reader.GetDouble(ordinal);
        }

        private static int? GetIntOrDefault(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
        }
    }

}