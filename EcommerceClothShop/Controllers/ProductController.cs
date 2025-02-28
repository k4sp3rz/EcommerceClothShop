using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EcommerceClothShop.Models;
using PagedList;

namespace EcommerceClothShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();
        private const int PageSize = 12; // Number of products per page

        // 🏪 List Products with Search, Filter & Pagination
        public ActionResult Index(string search, int? categoryId, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            var products = _context.Products.AsQueryable();

            // 🔍 Apply Filters
            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryID == categoryId.Value);

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            ViewBag.Categories = _context.Categories.ToList();

            // 📄 Paginate Products
            var paginatedProducts = products.OrderBy(p => p.ProductID).ToPagedList(page, PageSize);
            return View(paginatedProducts);
        }

        // 🛍 Product Details + Reviews
        public ActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();

            ViewBag.Reviews = _context.Reviews
                .Where(r => r.ProductID == id)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(product);
        }

        // ✍ Submit Product Review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(int productId, string reviewText, int rating)
        {
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to submit a review.";
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrWhiteSpace(reviewText) || rating < 1 || rating > 5)
            {
                TempData["ErrorMessage"] = "Invalid review. Please enter text and select a rating (1-5).";
                return RedirectToAction("Details", new { id = productId });
            }

            try
            {
                int userId = (int)Session["UserID"];
                var user = _context.Users.Find(userId);
                if (user == null) return RedirectToAction("Login", "Auth");

                _context.Reviews.Add(new Review
                {
                    ProductID = productId,
                    UserID = user.UserID,
                    Comment = reviewText.Trim(),
                    Rating = rating,
                    CreatedAt = DateTime.Now
                });

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Review submitted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "An error occurred while submitting your review.";
            }

            return RedirectToAction("Details", new { id = productId });
        }
    }
}
