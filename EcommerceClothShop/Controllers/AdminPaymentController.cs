using EcommerceClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceClothShop.Controllers
{
    public class AdminPaymentController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null || Session["UserRole"].ToString().ToLower() != "admin")
            {
                TempData["AuthError"] = "You must log in as an admin to access this page.";
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                { "controller", "Auth" },
                { "action", "Login" }
                    }
                );
            }
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            using (var db = new EcommerceClothShopEntities())
            {
                var payments = db.Payments.ToList();
                return View(payments);
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new EcommerceClothShopEntities())
            {
                var payment = db.Payments.Find(id);
                if (payment == null)
                {
                    return HttpNotFound();
                }
                return View(payment);
            }
        }

        public ActionResult Approve(int id)
        {
            using (var db = new EcommerceClothShopEntities())
            {
                var payment = db.Payments.Find(id);
                if (payment != null)
                {
                    payment.PaymentStatus = "Approved";
                    payment.PaidAt = DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult Reject(int id)
        {
            using (var db = new EcommerceClothShopEntities())
            {
                var payment = db.Payments.Find(id);
                if (payment != null)
                {
                    payment.PaymentStatus = "Rejected";
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
