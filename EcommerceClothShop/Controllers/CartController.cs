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
        public JsonResult AddToCart(int id)
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
                    cart.Add(new CartItem { Product = product, Quantity = 1 });
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
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
