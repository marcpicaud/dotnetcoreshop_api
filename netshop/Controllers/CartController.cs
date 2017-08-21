using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using netshop.Models;
using System.Linq;

namespace netshop.Controllers
{
	[Route("api/cart")]
	public class CartController : Controller
	{
		private readonly NetshopDbContext _context;

        // Constructor
		public CartController(NetshopDbContext context)
		{
			_context = context;
		}

        // Get all carts
		[HttpGet]
		public IEnumerable<Cart> GetAll()
		{
			return _context.Carts.ToList();
		}

		// Get a cart by ID
		[HttpGet("{id}", Name = "GetCart")]
		public IActionResult GetById(long id)
		{
			var item = _context.Carts.FirstOrDefault(t => t.CartId == id);
			if (item == null)
			{
				return NotFound();
			}
			return new ObjectResult(item);
		}

        // POST a new cart
		[HttpPost]
		public IActionResult Create([FromBody] Cart cart)
		{
			if (cart == null)
			{
				return BadRequest();
			}

            // Check if user exists
			User user = _context.Users.FirstOrDefault(t => t.UserId == cart.UserId);
			if (user == null)
			{
                return NotFound("User not found: " + user.UserId);
			}

            // Check if the products exist
            string[] productIdentifiers = cart.ProductIds.Split(",");
            for (int i = 0; i <productIdentifiers.Length; i++)
            {
                Product tmp = _context.Products.FirstOrDefault(t => t.ProductId == int.Parse(productIdentifiers[i]));
                if (tmp == null)
                {
                    return NotFound("Product not found : " + productIdentifiers[i]);
                }
            }

			_context.Carts.Add(cart);
			_context.SaveChanges();

			return CreatedAtRoute("GetCart", new { id = cart.CartId }, cart);
		}

        // PUT a cart
		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] Cart cart)
		{
			if (cart == null || cart.CartId != id)
			{
				return BadRequest();
			}

			var data = _context.Carts.FirstOrDefault(t => t.CartId == id);
			if (data == null)
			{
                return NotFound("Cart not found: " + id);
			}

			// Check if user exists
			User user = _context.Users.FirstOrDefault(t => t.UserId == cart.UserId);
			if (user == null)
			{
				return NotFound("User not found: " + user.UserId);
			}

			// Check if the products exist
			string[] productIdentifiers = cart.ProductIds.Split(",");
			for (int i = 0; i < productIdentifiers.Length; i++)
			{
				Product tmp = _context.Products.FirstOrDefault(t => t.ProductId == int.Parse(productIdentifiers[i]));
				if (tmp == null)
				{
					return NotFound("Product not found : " + productIdentifiers[i]);
				}
			}

            data.UserId = cart.UserId;
            data.ProductIds = cart.ProductIds;

			_context.Carts.Update(data);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var cart = _context.Carts.FirstOrDefault(t => t.CartId == id);
			if (cart == null)
			{
                return NotFound("Cart not found: " + id );
			}

			_context.Carts.Remove(cart);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
