using CSM.DataModel;
using CSM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSM.Controllers
{
	[Route("api/[controller]")]
    public class IncomeController : Controller
    {
		protected IInvoiceRepository repo;
		public IncomeController(IInvoiceRepository repository)
		{
			repo = repository;
		}

		// POST api/Income/5
		[HttpPost("{type}")]
		public async Task<IActionResult> GetIncomeByType(int type, [FromBody]DateTime body)
		{
			try
			{
				var result = await repo.GetIncomeByType((StatisticType)type, body);

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
