using System.Collections.Generic;

namespace EcommerceClothShop.Models
{
    public class OrderStatsViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        public List<OrdersPerMonthViewModel> OrdersPerMonth { get; set; }
    }

    public class OrdersPerMonthViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrderCount { get; set; }
    }
}
