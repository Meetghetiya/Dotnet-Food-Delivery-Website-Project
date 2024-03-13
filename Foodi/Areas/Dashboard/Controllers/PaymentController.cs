using Foodi.Areas.Admin.Models;
using Foodi.Areas.Dashboard.Models;
using Foodi.BAL;
using Foodi.DAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NToastNotify;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Borders;


namespace Foodi.Areas.Dashboard.Controllers
{
    [CheckUserAccess]
    [Area("Dashboard")]
    [Route("Dashboard/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly Cart_BalBase _cartbalbase;
        private readonly IToastNotification _toastNotification;
        private readonly Payment_BalBase _paymentService;
        private readonly User_BalBase _userbalbase;
        private readonly Order_DalBase _OrderService;
        public PaymentController(IConfiguration configuration, IToastNotification toastNotification)
        {
            this._cartbalbase = new Cart_BalBase();
            this._paymentService = new Payment_BalBase();
            this._OrderService = new Order_DalBase();
            _userbalbase = new User_BalBase();
            _toastNotification = toastNotification;
        }
        public IActionResult Payment()
        {
            var GrandTotal = 0;
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);
            List<Cart> cart = _cartbalbase.FilterCart(UserId: userID);
            foreach (Cart c in cart)
            {
                GrandTotal += c.Price * c.Quantity;
            }

            ViewBag.GrandTotal = GrandTotal;

            User user = _userbalbase.GetUserById(userID);
            ViewBag.Address = user.Address;
            return View();
        }

        [HttpPost]
        public IActionResult AddPayment(Payment payment)
        {
            string errorMessage;
            int PaymentId = 0;
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);
            payment.UserId = userID;

            if (payment.PaymentMode == "CashOnDelivery")
            {
                if (payment.Address == null)
                {
                    _toastNotification.AddInfoToastMessage("Address is not given");
                    return View("Payment");
                }
                else
                {
                    PaymentId = _paymentService.AddPayment(payment);
                }

            }
            else if (ValidatePayment(payment, out errorMessage))
            {
                string expirationDate = $"{payment.ExpirationMonth}/{payment.ExpirationYear}";
                payment.ExpiryDate = expirationDate;
                PaymentId = _paymentService.AddPayment(payment);

            }
            else
            {
                _toastNotification.AddInfoToastMessage(errorMessage);
                return View("Payment");
            }

            List<Cart> cart = _cartbalbase.FilterCart(UserId: userID);

            Order order = new Order
            {
                OrderNo = GenerateOrderNumber(),
                UserId = userID,
                Status = "Pending",
                PaymentId = PaymentId, // Use the retrieved PaymentId
                OrderDate = DateTime.Now,
                Address = payment.Address,
            };

            int OrderId = _OrderService.InsertOrder(order);
            foreach (Cart cart2 in cart)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = OrderId,
                    UserId = userID,
                    ProductId = cart2.ProductId,
                    Quantity = cart2.Quantity,
                    PaymentId = PaymentId
                };
                _OrderService.InsertOrderItem(orderItem);
            }

            return RedirectToAction("PaymentInvoice", new { OrderId });
        }


        [HttpGet]
        public IActionResult GetOrderItems(int orderId)
        {
            try
            {
                var orderItems = _OrderService.GetOrderItems(orderId);

                return Json(new { success = true, orderItems });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult PaymentInvoice(int OrderId)
        {


            Order Orderdetail = _OrderService.GetOrdersByOrderId(OrderId);
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);
            _cartbalbase.DeleteCartByUserId(userID);

            return View(Orderdetail);
        }

        [HttpGet]
        public IActionResult GeneratePdf(int orderId)
        {
            List<OrderItem> orderItems = _OrderService.GetOrderItems(orderId);
            Order Orderdetail = _OrderService.GetOrdersByOrderId(orderId);

            // Generate PDF
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new iText.Layout.Document(pdf);

                        // Create a table for the header
                        var headerTable = new Table(1);
                        headerTable.SetWidth(UnitValue.CreatePercentValue(100));

                        // Row 1: Food Deliver
                        var cell1 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER);
                        cell1.Add(new Paragraph("Your Invoice"));
                        headerTable.AddCell(cell1);

                        // Row 2: From
                        var cell2 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER);
                        cell2.Add(new Paragraph("From: Foodi "));
                        headerTable.AddCell(cell2);

                        // Row 3: Date
                        var cell3 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER);
                        cell3.Add(new Paragraph($"Date: {Orderdetail.OrderDate.ToString("yyyy-MM-dd")}"));
                        headerTable.AddCell(cell3);

                        document.Add(headerTable);

                        document.Add(new LineSeparator(new SolidLine()));

                        document.Add(new Paragraph($"\nOrder No  : {Orderdetail.OrderNo.ToString()}  ").SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph($"\nUserName : {Orderdetail.UserName.ToString()}  ").SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph("\nOrder Items").SetTextAlignment(TextAlignment.CENTER));

                        var table = new Table(5);
                        table.SetMarginTop(30f);

                        table.AddHeaderCell(new Cell().Add(new Paragraph("Count").SetPadding(10f)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Product Name").SetPadding(10f)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").SetPadding(10f)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Price").SetPadding(10f)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Total").SetPadding(10f)));

                        decimal grandTotal = 0;
                        int count = 1;

                        foreach (OrderItem item in orderItems)
                        {
                            var total = item.Quantity * item.Price;
                            grandTotal += total;
                           
                            table.AddCell(new Cell().Add(new Paragraph(count.ToString()).SetPadding(10f)));
                            table.AddCell(new Cell().Add(new Paragraph(item.ProductName).SetPadding(10f)));
                            table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString()).SetPadding(10f)));
                            table.AddCell(new Cell().Add(new Paragraph(item.Price.ToString("C")).SetPadding(10f)));
                            table.AddCell(new Cell().Add(new Paragraph(total.ToString("C")).SetPadding(10f)));
                            count++;
                        }
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        document.Add(table);

                        var grandTotalRowUnder = new Table(5).SetBorder(Border.NO_BORDER);
     

                      

                        grandTotalRowUnder.AddCell(new Cell().Add(new Paragraph("Grand Total:").SetPadding(10f).SetBorder(Border.NO_BORDER)));
                        grandTotalRowUnder.AddCell(new Cell().Add(new Paragraph(grandTotal.ToString("C")).SetPadding(10f).SetBorder(Border.NO_BORDER))); // Format as currency

                        grandTotalRowUnder.SetMarginLeft(284f);
                        document.Add(grandTotalRowUnder);
                    }
                }
                var pdfBytes = memoryStream.ToArray();
                return new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = "OrderItems.pdf"
                };
            }




        }



        public bool ValidatePayment(Payment payment, out string errorMessage)
        {

            errorMessage = string.Empty;

            if (!System.Text.RegularExpressions.Regex.IsMatch(payment.CardNo, @"^\d{16}$"))
            {
                errorMessage = "Invalid card number format.";
                return false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(payment.CvvNo, @"^\d{3}$"))
            {
                errorMessage = "Invalid CVV number format.";
                return false;
            }


            return true;
        }

        public string GenerateOrderNumber()
        {
            string prefix = "ORD";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();

            string orderNumber = $"{prefix}-{datePart}-{randomPart}";

            return orderNumber;
        }
    }
}
