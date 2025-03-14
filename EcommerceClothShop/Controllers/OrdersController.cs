using EcommerceClothShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceClothShop.Controllers
{
    public class OrdersController : Controller
    {
        private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        public ActionResult OrderHistory()
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = db.Orders
                           .Where(o => o.UserID == userId)
                           .OrderByDescending(o => o.CreatedAt)
                           .ToList();
            return View(orders);
        }

        public ActionResult OrderDetails(int id)
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = db.Orders
                          .Include("OrderDetails.Product")
                          .Include("Payments") // Load payment details
                          .FirstOrDefault(o => o.OrderID == id && o.UserID == userId);

            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.OrderTotal = order.OrderDetails.Sum(od => od.Quantity * od.Product.Price);
            return View(order);
        }

        [HttpPost]
        public ActionResult ConfirmOrder(int id, string paymentMethod)
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = db.Orders
                          .Include("OrderDetails.Product")
                          .FirstOrDefault(o => o.OrderID == id && o.UserID == userId);

            if (order == null)
            {
                return HttpNotFound("Order not found.");
            }

            // Prevent duplicate confirmation
            if (order.OrderStatus == "Confirmed")
            {
                TempData["Message"] = "Order has already been confirmed.";
                return RedirectToAction("OrderDetails", new { id = order.OrderID });
            }

            // Check stock availability
            foreach (var detail in order.OrderDetails)
            {
                var product = db.Products.Find(detail.ProductID);
                if (product.Stock < detail.Quantity)
                {
                    ModelState.AddModelError("", $"Insufficient stock for {product.Name}. Available: {product.Stock}");
                    ViewBag.OrderTotal = order.OrderDetails.Sum(od => od.Quantity * od.Product.Price);
                    return View("OrderDetails", order);
                }
            }

            // Deduct stock
            foreach (var detail in order.OrderDetails)
            {
                var product = db.Products.Find(detail.ProductID);
                product.Stock -= detail.Quantity;
            }

            // Update Order Status
            order.OrderStatus = "Confirmed";

            // Process Payment
            var payment = db.Payments.FirstOrDefault(p => p.OrderID == order.OrderID);
            if (payment == null)
            {
                payment = new Payment
                {
                    OrderID = order.OrderID,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = "Paid",
                    PaidAt = DateTime.Now
                };
                db.Payments.Add(payment);
            }
            else
            {
                payment.PaymentStatus = "Paid";
                payment.PaidAt = DateTime.Now;
            }

            db.SaveChanges();

            TempData["Message"] = "Order confirmed and payment recorded.";
            return RedirectToAction("OrderDetails", new { id = order.OrderID });
        }

        public ActionResult Invoice(int id)
        {
            int? userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = db.Orders
                          .Include("OrderDetails.Product")
                          .Include("Payments") // Load payment details
                          .FirstOrDefault(o => o.OrderID == id && o.UserID == userId);

            if (order == null)
            {
                return HttpNotFound("Order not found.");
            }

            ViewBag.OrderTotal = order.OrderDetails.Sum(od => od.Quantity * od.Product.Price);
            return View("Invoice", order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
