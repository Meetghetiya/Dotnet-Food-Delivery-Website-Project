using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foodi.Areas.Admin.Models
{

    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name must be less than 255 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please specify whether the product is active or not.")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }

    public class MenuViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }

}
