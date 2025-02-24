using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EcommerceClothShop.Models;

namespace EcommerceClothShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

        // Display all products
        public ActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
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
