using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MinLength(5)]
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
