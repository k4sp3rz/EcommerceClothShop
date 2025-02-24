using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using EcommerceClothShop.Models;

public class AuthController : Controller
{
    private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

    // GET: Login Page
    public ActionResult Login()
    {
        return View();
    }

    // POST: Login Logic
    [HttpPost]
    public ActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user != null && VerifyPassword(password, user.PasswordHash))
        {
            FormsAuthentication.SetAuthCookie(user.Email, false);

            Session["UserID"] = user.UserID;
            Session["UserRole"] = user.Role;
            Session["UserName"] = user.FullName;
            Session["UserEmail"] = user.Email;

            if (user.Role == "admin")
                return RedirectToAction("Dashboard", "Admin");
            else
                return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid email or password.";
        return View();
    }
    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Register(User newUser, string ConfirmPassword)
    {
        // Check if email already exists
        if (_context.Users.Any(u => u.Email == newUser.Email))
        {
            ViewBag.ErrorMessage = "Email is already registered.";
            return View();
        }

        // Check password confirmation
        if (newUser.PasswordHash != ConfirmPassword)
        {
            ViewBag.ErrorMessage = "Passwords do not match.";
            return View();
        }

        // Assign default role
        newUser.Role = "customer";
        newUser.CreatedAt = DateTime.Now;

        // Save new user
        _context.Users.Add(newUser);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Registration successful! Please log in.";
        return RedirectToAction("Login");
    }

    // Logout
    public ActionResult Logout()
    {
        FormsAuthentication.SignOut();
        Session.Clear();
        return RedirectToAction("Login");
    }

    // Helper: Verify Password (replace with hashing logic)
    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        return inputPassword == storedHash; // Replace with a real hash check
    }
}
