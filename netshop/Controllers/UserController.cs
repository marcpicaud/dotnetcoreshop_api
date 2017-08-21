using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using netshop.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Net;

namespace netshop.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly NetshopDbContext _context;

        public UserController(NetshopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var item = _context.Users.FirstOrDefault(t => t.UserId == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpGet("{token}")]
        [Route("Token")]
        public IActionResult GetByToken(string token)
        {
            var item = _context.Users.FirstOrDefault(t => t.Token == token);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var item = _context.Users.FirstOrDefault(t => t.Login == user.Login && t.Password == user.Password );
            if (item == null)
            {
				return StatusCode(403, "Invalid login/password");
			}
            return new ObjectResult(item);
        }

		[HttpPost]
		public IActionResult Create([FromBody] User user)
		{
			if (user == null)
			{
				return BadRequest();
			}

            // TODO : No time to implement proper encryption but you get the idea
            user.Token = user.Firstname + user.Lastname;

			_context.Users.Add(user);
			_context.SaveChanges();

			return CreatedAtRoute("GetUser", new { id = user.UserId }, user);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] User user)
		{
			if (user == null || user.UserId != id)
			{
				return BadRequest();
			}

			var data = _context.Users.FirstOrDefault(t => t.UserId == id);
			if (data == null)
			{
				return NotFound();
			}

            data.Firstname = user.Firstname;
            data.Lastname = user.Lastname;
            data.Login = user.Login;
            data.Password = user.Password;
            data.Admin = user.Admin;

			_context.Users.Update(data);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var user = _context.Users.FirstOrDefault(t => t.UserId == id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			_context.SaveChanges();
			return new NoContentResult();
		}

	}
}
