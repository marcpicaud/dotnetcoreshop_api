using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using netshop.Models;
using System.Linq;

namespace netshop.Controllers
{
	[Route("api/category")]
	public class CategoryController : Controller
	{
		private readonly NetshopDbContext _context;

		public CategoryController(NetshopDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IEnumerable<Category> GetAll()
		{
			return _context.Categories.ToList();
		}

		[HttpGet("{id}", Name = "GetCategory")]
		public IActionResult GetById(long id)
		{
			var item = _context.Categories.FirstOrDefault(t => t.CategoryId == id);
			if (item == null)
			{
				return NotFound();
			}
			return new ObjectResult(item);
		}

		[HttpPost]
		public IActionResult Create([FromBody] Category category)
		{
			if (category == null)
			{
				return BadRequest();
			}

			_context.Categories.Add(category);
			_context.SaveChanges();

			return CreatedAtRoute("GetCategory", new { id = category.CategoryId }, category);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] Category category)
		{
			if (category == null || category.CategoryId != id)
			{
				return BadRequest();
			}

			var data = _context.Categories.FirstOrDefault(t => t.CategoryId == id);
			if (data == null)
			{
				return NotFound();
			}

			data.Name = category.Name;

			_context.Categories.Update(data);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var category = _context.Categories.FirstOrDefault(t => t.CategoryId == id);
			if (category == null)
			{
				return NotFound();
			}

			_context.Categories.Remove(category);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
