using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceClothShop.Models;
using EcommerceClothShop.Library;

namespace EcommerceClothShop.Controllers
{
    public class CartController : Controller
    {
        private readonly EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        private List<CartItem> GetCart()
        {
            return Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        }

        public ActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        public JsonResult AddToCart(int id, int quantity)
        {
            var cart = GetCart();
            var product = db.Products.Find(id);

            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.Product.ProductID == id);
                if (product.Stock < quantity + (existingItem?.Quantity ?? 0))
                {
                    return Json(new { success = false, message = "Not enough stock available." });
                }

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem { Product = product, Quantity = quantity });
                }
                Session["Cart"] = cart;
                Session["CartCount"] = cart.Sum(c => c.Quantity);
            }
            return Json(new { success = true, cartCount = Session["CartCount"] });
        }

        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
            {
                TempData["CheckoutError"] = "You must log in to proceed.";
                return RedirectToAction("Login", "Auth");
            }

            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["CartError"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        public ActionResult PlaceOrder(string paymentMethod)
        {
            var allowedMethods = new List<string> { "cod", "bank_transfer", "paypal", "credit_card", "vnpay" };
            if (!allowedMethods.Contains(paymentMethod?.ToLower()))
            {
                TempData["CheckoutError"] = "Invalid payment method.";
                return RedirectToAction("Checkout");
            }

            var cart = GetCart();
            int? userId = Session["UserID"] as int?;
            if (cart == null || !cart.Any() || userId == null)
            {
                TempData["CartError"] = "Your cart is empty or you must log in to proceed.";
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
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
                        db.OrderDetails.Add(new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ProductID = item.Product.ProductID,
                            Quantity = item.Quantity,
                            Price = item.Product.Price
                        });
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    if (paymentMethod.ToLower() == "vnpay")
                    {
                        return Redirect(GenerateVNPayUrl(order));
                    }

                    Session["Cart"] = null;
                    Session["CartCount"] = 0;
                    return RedirectToAction("OrderConfirmation", new { orderId = order.OrderID });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine("Order placement error: " + ex.Message);
                    TempData["CheckoutError"] = "Something went wrong while placing the order.";
                    return RedirectToAction("Checkout");
                }
            }
        }

        private string GenerateVNPayUrl(Order order)
        {
            string vnpUrl = ConfigurationManager.AppSettings["vnp_Url"];
            string vnpTmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnpHashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            string vnpReturnUrl = Url.Action("VNPayReturn", "Cart", null, Request.Url.Scheme);
            string ipAddr = Utils.GetIpAddress();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(order.TotalAmount * 100)).ToString());
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.OrderID.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Payment for Order #{order.OrderID}");
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnpReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", ipAddr);

            // Time settings (VNPay requires specific format)
            string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vnpay.AddRequestData("vnp_CreateDate", createDate);

            return vnpay.CreateRequestUrl(vnpUrl, vnpHashSecret);
        }
        public ActionResult VNPayReturn()
        {
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            var vnpay = new VnPayLibrary();

            foreach (string key in Request.QueryString.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    vnpay.AddResponseData(key, Request.QueryString[key]);
                }
            }

            string vnpSecureHash = Request.QueryString["vnp_SecureHash"];
            bool isValidSignature = vnpay.ValidateSignature(vnpSecureHash, vnp_HashSecret);

            if (isValidSignature)
            {
                string responseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string transactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                int orderId = int.Parse(vnpay.GetResponseData("vnp_TxnRef"));

                if (responseCode == "00" && transactionStatus == "00")
                {
                    var order = db.Orders.Find(orderId);
                    if (order != null)
                    {
                        order.OrderStatus = "Paid";
                        db.SaveChanges();
                    }

                    Session["Cart"] = null;
                    Session["CartCount"] = 0;
                    return RedirectToAction("OrderConfirmation", new { orderId });
                }
                else
                {
                    TempData["CheckoutError"] = "Payment failed. Please try again.";
                }
            }
            else
            {
                TempData["CheckoutError"] = "Invalid payment response.";
            }
            return RedirectToAction("Checkout");
        }

        public ActionResult OrderConfirmation(int orderId)
        {
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return HttpNotFound();
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
