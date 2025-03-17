using EcommerceClothShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceClothShop.Controllers
{
    public class AdminDiscountController : Controller
    {
        private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

        // Admin session verification
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "admin")
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            base.OnActionExecuting(filterContext);
        }

        // ---------------- PRODUCT DISCOUNT SECTION ----------------

        public ActionResult ProductDiscounts()
        {
            var discounts = _context.ProductDiscounts.Include("Product").ToList();
            return View(discounts);
        }

        public ActionResult CreateProductDiscount()
        {
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsDeleted != true), "ProductID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProductDiscount(ProductDiscount discount)
        {
            if (ModelState.IsValid)
            {
                _context.ProductDiscounts.Add(discount);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product discount created successfully!";
                return RedirectToAction("ProductDiscounts");
            }
            ViewBag.Products = new SelectList(_context.Products.Where(p => (bool)!p.IsDeleted), "ProductID", "Name", discount.ProductID);
            return View(discount);
        }

        public ActionResult EditProductDiscount(int id)
        {
            var discount = _context.ProductDiscounts.Find(id);
            if (discount == null) return HttpNotFound();
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsDeleted != true), "ProductID", "Name", discount.ProductID);
            return View(discount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductDiscount(ProductDiscount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(discount).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product discount updated successfully!";
                return RedirectToAction("ProductDiscounts");
            }
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsDeleted != true), "ProductID", "Name", discount.ProductID);
            return View(discount);
        }

        [HttpPost]
        public ActionResult DeleteProductDiscount(int id)
        {
            var discount = _context.ProductDiscounts.Find(id);
            if (discount != null)
            {
                _context.ProductDiscounts.Remove(discount);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductDiscounts");
        }

        // ---------------- PROMO CODE SECTION ----------------

        public ActionResult PromoCodes()
        {
            var promos = _context.DiscountPromos.ToList();
            return View(promos);
        }

        public ActionResult CreatePromoCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePromoCode(DiscountPromo promo)
        {
            if (ModelState.IsValid)
            {
                _context.DiscountPromos.Add(promo);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Promo code created successfully!";
                return RedirectToAction("PromoCodes");
            }
            return View(promo);
        }

        public ActionResult EditPromoCode(int id)
        {
            var promo = _context.DiscountPromos.Find(id);
            if (promo == null) return HttpNotFound();
            return View(promo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPromoCode(DiscountPromo promo)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(promo).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Promo code updated successfully!";
                return RedirectToAction("PromoCodes");
            }
            return View(promo);
        }

        [HttpPost]
        public ActionResult DeletePromoCode(int id)
        {
            var promo = _context.DiscountPromos.Find(id);
            if (promo != null)
            {
                _context.DiscountPromos.Remove(promo);
                _context.SaveChanges();
            }
            return RedirectToAction("PromoCodes");
        }
    }
}
