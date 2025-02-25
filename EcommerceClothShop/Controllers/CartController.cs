using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EcommerceClothShop.Models;

namespace EcommerceClothShop.Controllers
{
    public class CartController : Controller
    {
        private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        // Display the Cart
        public ActionResult Index()
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public JsonResult AddToCart(int id, int quantity)
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            var product = db.Products.Find(id);

            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.Product.ProductID == id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = product, Quantity = quantity });
                }

                // Update session
                Session["Cart"] = cart;
                Session["CartCount"] = cart.Sum(c => c.Quantity);
            }

            return Json(new { success = true, cartCount = Session["CartCount"] });
        }

        [HttpPost]
        public JsonResult UpdateCart(int id, int quantity)
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.Product.ProductID == id);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Remove(item);
                }

                // Update session
                Session["Cart"] = cart;
                Session["CartCount"] = cart.Sum(c => c.Quantity);
            }

            return Json(new
            {
                success = true,
                total = cart.Sum(c => c.Product.Price * c.Quantity),
                cartCount = Session["CartCount"]
            });
        }

        [HttpPost]
        public JsonResult RemoveFromCart(int id)
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.Product.ProductID == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            // Update session
            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(c => c.Quantity);

            return Json(new
            {
                success = true,
                total = cart.Sum(c => c.Product.Price * c.Quantity),
                cartCount = Session["CartCount"]
            });
        }

        [HttpGet]
        public JsonResult GetCartCount()
        {
            return Json(Session["CartCount"] ?? 0, JsonRequestBehavior.AllowGet);
        }

        // 🔹 Require Login for Checkout
        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
            {
                TempData["CheckoutError"] = "You must log in to proceed with the purchase.";
                return RedirectToAction("Login", "Auth");
            }

            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["CartError"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            return View(cart);
        }
        [HttpPost]
        public ActionResult PlaceOrder(string paymentMethod)
        {
            var cart = Session["Cart"] as List<CartItem>;
            int? userId = Session["UserID"] as int?;

            if (cart == null || !cart.Any())
            {
                TempData["CartError"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            if (userId == null)
            {
                TempData["CheckoutError"] = "You must log in to proceed.";
                return RedirectToAction("Login", "Auth");
            }

            Order order;

            using (var db = new EcommerceClothShopEntities())
            {
                order = new Order
                {
                    UserID = userId.Value,
                    TotalAmount = cart.Sum(item => item.Product.Price * item.Quantity),
                    OrderStatus = "Pending",
                    CreatedAt = DateTime.Now
                };

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.Product.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };

                    db.OrderDetails.Add(orderDetail);
                }
                var payment = new Payment
                {
                    OrderID = order.OrderID,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = paymentMethod == "COD" ? "Pending" : "Paid",
                    PaidAt = paymentMethod == "COD" ? (DateTime?)null : DateTime.Now
                };

                db.Payments.Add(payment);
                db.SaveChanges();
            }

            Session["Cart"] = null;
            Session["CartCount"] = 0;

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderID });
        }
        public ActionResult OrderConfirmation(int orderId)
        {
            var order = db.Orders.Include("OrderDetails.Product").FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Shop");
            }

            return View(order);
        }

    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}
