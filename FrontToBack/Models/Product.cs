using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required ]
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public float Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }
        public List<SaleProduct> SaleProducts { get; set; }
    }
}
