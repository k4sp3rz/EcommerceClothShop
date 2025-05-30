﻿using EcommerceClothShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceClothShop.Controllers
{
    public class AdminController : Controller
    {
        private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "admin")
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Dashboard()
        {
            var ordersPerMonth = db.Orders
                .Where(o => o.CreatedAt.HasValue)
                .GroupBy(o => new { o.CreatedAt.Value.Year, o.CreatedAt.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();

            ViewBag.Months = ordersPerMonth.Select(o => $"{o.Month}/{o.Year}").ToList();
            ViewBag.OrderCounts = ordersPerMonth.Select(o => o.OrderCount).ToList();

            return View();
        }

        public ActionResult Orders()
        {
            var orders = db.Orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(orders);
        }

        // 🔹 View Order Details
        public ActionResult OrderDetails(int id)
        {
            var order = db.Orders
                          .Include("OrderDetails.Product")  // Ensure Product is loaded
                          .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound("Order not found.");
            }

            ViewBag.PaymentStatus = order.Payments?.FirstOrDefault()?.PaymentStatus ?? "Not Paid";
            return View(order);
        }

        // 🔹 Update Order Status
        [HttpPost]
        public ActionResult UpdateOrderStatus(int id, string newStatus)
        {
            var order = db.Orders
                          .Include("Payments")
                          .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound("Order not found.");
            }

            order.OrderStatus = newStatus;

            // 🔹 Update Payment Status if needed
            var payment = db.Payments.FirstOrDefault(p => p.OrderID == order.OrderID);
            if (payment != null)
            {
                if (newStatus == "Canceled" || newStatus == "Refunded")
                {
                    payment.PaymentStatus = "Refunded";
                }
                else if (newStatus == "Completed")
                {
                    payment.PaymentStatus = "Paid";
                }
            }

            db.SaveChanges();
            TempData["Message"] = "Order status updated successfully.";
            return RedirectToAction("OrderDetails", new { id = order.OrderID });
        }

        public ActionResult Stats()
        {
            var totalRevenue = db.Orders.Where(o => o.OrderStatus == "Completed").Sum(o => (decimal?)o.TotalAmount) ?? 0;
            var totalOrders = db.Orders.Count();
            var completedOrders = db.Orders.Count(o => o.OrderStatus == "Completed");
            var pendingOrders = db.Orders.Count(o => o.OrderStatus == "Pending");

            var ordersPerMonth = db.Orders
                .Where(o => o.CreatedAt.HasValue)
                .GroupBy(o => new { o.CreatedAt.Value.Year, o.CreatedAt.Value.Month })
                .Select(g => new OrdersPerMonthViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    OrderCount = g.Count()
                })
                .OrderByDescending(g => g.Year)
                .ThenByDescending(g => g.Month)
                .Take(6)
                .ToList();

            var stats = new OrderStatsViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                CompletedOrders = completedOrders,
                PendingOrders = pendingOrders,
                OrdersPerMonth = ordersPerMonth
            };

            return View(stats);
        }
    }
}
