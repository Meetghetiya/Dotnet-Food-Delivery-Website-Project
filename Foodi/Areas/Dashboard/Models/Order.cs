using System.ComponentModel.DataAnnotations;

namespace Foodi.Areas.Dashboard.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderNo { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        public int? PaymentId { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } 
    }
}
