using System;
using System.Collections.Generic;

namespace netshop.Models
{
	public class Cart
	{
		public int CartId { get; set; }

		public int UserId { get; set; }
        public string ProductIds { get; set; }
	}
}
