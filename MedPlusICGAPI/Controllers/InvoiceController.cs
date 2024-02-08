using MedPlusICGAPI.DTOs;
using MedPlusICGAPI.Helpers;
using MedPlusICGAPI.Model;
using MedPlusICGAPI.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MedPlusICGAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepo _invoiceRepo;

        public InvoiceController(IInvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        /// <summary>
        /// Retrieves an invoice by its invoice number and serial number.
        /// </summary>
        /// <param name="getInvoiceRequest">The request object containing the invoice number and serial number.</param>
        /// <response code="200">Returns the requested invoice.</response>
        /// <response code="500">If an error occurs while processing the request.</response>
        [HttpPost("invoice")]
        [ProducesResponseType(typeof(Invoice), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvoiceBySerieAndInvoiceNumber(GetInvoiceRequest getInvoiceRequest)
        {
            try
            {
                var result = await _invoiceRepo.InvoiceByInvoiceNumberNSerie(getInvoiceRequest.InvoiceNumber, getInvoiceRequest.Serie);
                return Ok(result);
            }
            catch (NoDataFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
