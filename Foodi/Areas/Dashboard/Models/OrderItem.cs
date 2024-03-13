using Foodi.Areas.Admin.Models;
using Foodi.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodi.Areas.Dashboard.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int UserId { get; set; }


        [Required]
        public decimal Price { get; set; }

        [Required]
        public int PaymentId { get; set; }


    }

    public class UserOrder
    {
        public User User { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class UserOrderTab
    {
        public User User { get; set; }

        public List<Order> Orders { get; set; }

        public bool ActivateConnectedServicesTab { get; set; }

    }

    public class OrderDetail
    {
        public Order Order { get; set; }

        public List<OrderItem> OrderItem { get; set; }
    }
}
