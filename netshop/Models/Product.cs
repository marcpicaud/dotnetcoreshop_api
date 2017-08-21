using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netshop.Models
{
    public class Product
    {
		public long ProductId { get; set; }
		[Required]
        public string Name { get; set; }
		[Required]
        public double Price { get; set; }

        public int CategoryId { get; set; }
    }
}
