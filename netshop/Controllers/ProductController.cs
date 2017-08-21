using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using netshop.Models;
using System.Linq;

namespace netshop.Controllers
{
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly NetshopDbContext _context;

        public ProductController(NetshopDbContext context)
        {
            _context = context;
		}

		[HttpGet]
		public IEnumerable<Product> GetAll()
		{
			return _context.Products.ToList();
		}

		[HttpGet("{id}", Name = "GetProduct")]
		public IActionResult GetById(long id)
		{
			var item = _context.Products.FirstOrDefault(t => t.ProductId == id);
			if (item == null)
			{
				return NotFound();
			}
			return new ObjectResult(item);
		}

		[HttpPost]
		public IActionResult Create([FromBody] Product product)
		{
			if (product == null)
			{
				return BadRequest("Couldn't create product from request body");
			}

            // Check if category exists
            Category category = _context.Categories.FirstOrDefault(t => t.CategoryId == product.CategoryId);
            if (category == null)
            {
                return NotFound("Missing Category");
            }
			
            _context.Products.Add(product);
            _context.SaveChanges();

			return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] Product product)
		{
			if (product == null || product.ProductId != id)
			{
				return BadRequest("product not found");
			}

			var data = _context.Products.FirstOrDefault(t => t.ProductId == id);
			if (data == null)
			{
				return NotFound();
			}

			// Check if category exists
			Category category = _context.Categories.FirstOrDefault(t => t.CategoryId == product.CategoryId);
			if (category == null)
			{
				return NotFound("Missing Category");
			}

			data.Name = product.Name;
            data.CategoryId = product.CategoryId;
            data.Price = product.Price;

			_context.Products.Update(data);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var product = _context.Products.FirstOrDefault(t => t.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			_context.Products.Remove(product);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
