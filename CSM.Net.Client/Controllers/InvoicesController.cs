using CSM.DataModel;
using CSM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSM.Controllers
{
	[Route("api/[controller]")]
    public class InvoicesController : Controller
    {
		protected IInvoiceRepository repo;
		public InvoicesController(IInvoiceRepository repository)
		{
			repo = repository;
		}

		// GET api/tables
		[HttpGet]
		public async Task<IActionResult> GetInvoices()
		{
			try
			{
				var result = await repo.GetInvoices();

				//request is ok
				return Ok(result);
			}
			catch (Exception ex)
			{
				//TODO: log error

				//internal server error
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

		// GET api/tables/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetInvoiceByID(int id)
		{
			try
			{
				var result = await repo.GetInvoiceByID(id);

				//request is ok
				return Ok(result);
			}
			catch (Exception ex)
			{
				//TODO: log error

				//internal server error
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}

        // POST api/values/5
        [HttpPost("{id}")]
        public async Task<IActionResult> CreateNewInvoice(int id)
		{
			try
			{
				var result = await repo.CreateNewInvoice(id);

				//request is ok
				return Ok(result);
			}
			catch (Exception ex)
			{
				//TODO: log error

				//internal server error
				return StatusCode(StatusCodes.Status500InternalServerError, ex);
			}
		}
    }
}
