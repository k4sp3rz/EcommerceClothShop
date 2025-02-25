using EcommerceClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var order = db.Orders
                          .Include("OrderDetails.Product")
                          .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
    }

}