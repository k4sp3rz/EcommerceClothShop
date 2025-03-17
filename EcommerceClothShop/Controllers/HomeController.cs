using EcommerceClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceClothShop.Controllers
{
    public class HomeController : Controller
    {
        private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        public ActionResult Index()
        {
            var products = db.Products
                .Include("Category")
                .Where(p => p.IsDeleted != true)
                .ToList();

            return View(products); // Now Model will not be null
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}