using System;
namespace netshop.Models
{
    public class User
    {
		public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }
        public bool Admin { get; set; }
    }
}
