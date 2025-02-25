using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using EcommerceClothShop.Models;

namespace EcommerceClothShop.Controllers
{
    public class AdminStatsController : Controller
    {
        private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

        // GET: AdminStats
        public ActionResult Index()
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
