using CSM.DataModel;
using CSM.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Controllers
{
	[Route("api/[controller]")]
    public class ProductsController : Controller
    {
		protected IProductRepository repo;
		public ProductsController(IProductRepository repository)
		{
			repo = repository;
		}

		// GET api/products
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			try
			{
				var result = await repo.GetProducts();

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


        // POST api/values
        [HttpPost]
		public async Task<IActionResult> AddNewProduct([FromBody]Product product)
		{
			try
			{
				var result = await repo.AddNewProduct(product);

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
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]Product product)
		{
			try
			{
				var result = await repo.UpdateProduct(id, product);

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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var result = await repo.RemoveProduct(id);

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
