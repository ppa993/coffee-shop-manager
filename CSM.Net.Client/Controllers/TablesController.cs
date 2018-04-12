using CSM.DataModel;
using CSM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CSM.Controllers
{
	[Route("api/[controller]")]
    public class TablesController : Controller
    {
		protected ITableRepository repo;
		public TablesController(ITableRepository repository)
		{
			repo = repository;
		}

		// GET api/tables
		[HttpGet]
		public async Task<IActionResult> GetTables()
		{
			try
			{
				var result = await repo.GetTables();

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
		public async Task<IActionResult> GetTableByID(int id)
		{
			try
			{
				var result = await repo.GetTableByID(id);

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
        public async Task<IActionResult> MoveTable(int id, [FromBody]int targetID)
		{
			try
			{
				var result = await repo.MoveTable(id, targetID);

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

        // PUT api/values/5
        [HttpPut("{id}")]
		public async Task<IActionResult> UpdateTableProducts(int id, [FromBody]MoveTableModel body)
		{
			try
			{
				var result = await repo.UpdateTableProducts(id, body.productID, (UpdateAction)body.action, body.targetID);

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
