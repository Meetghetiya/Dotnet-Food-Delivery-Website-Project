using System.ComponentModel.DataAnnotations;

namespace Foodi.Areas.Admin.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        public int Price { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
