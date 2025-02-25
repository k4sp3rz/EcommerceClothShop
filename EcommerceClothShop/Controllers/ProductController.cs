using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EcommerceClothShop.Models;

namespace EcommerceClothShop.Controllers
{
    public class ProductController : Controller
    {
        private EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

        public ActionResult Index(string search, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var products = _context.Products.AsQueryable();

            // 🔍 Filter by Search
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search));
            }

            // 📂 Filter by Category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
            }

            // 💲 Filter by Price Range
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // Fetch categories for dropdown
            ViewBag.Categories = _context.Categories.ToList();

            return View(products.ToList());
        }

        public ActionResult Details(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return HttpNotFound();
        }

        // Explicitly load User entity to avoid proxy issues
        var reviews = _context.Reviews
                              .Where(r => r.ProductID == id)
                              .Include(r => r.User) // Ensure User data is included
                              .OrderByDescending(r => r.CreatedAt)
                              .ToList(); // Convert to a List before passing to ViewBag

        ViewBag.Reviews = reviews; // Store the list in ViewBag

        return View(product);
    }



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

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found. Please log in again.";
                    return RedirectToAction("Login", "Auth");
                }

                var review = new Review
                {
                    ProductID = productId,
                    UserID = user.UserID,
                    Comment = reviewText.Trim(),
                    Rating = rating,
                    CreatedAt = DateTime.Now
                };

                _context.Reviews.Add(review);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Review submitted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "An error occurred while submitting your review. Please try again.";
            }

            return RedirectToAction("Details", new { id = productId });
        }
    }
}
