using EcommerceClothShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AdminUserController : Controller
{


    public ActionResult ManageUsers()
    {
        using (var db = new EcommerceClothShopEntities()) // Replace with your actual DbContext
        {
            List<User> users = db.Users.ToList();
            return View(users);
        }
    }

    public ActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public ActionResult CreateUser(User user)
    {
        using (var db = new EcommerceClothShopEntities())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        return RedirectToAction("ManageUsers");
    }

    private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

    // ✅ Show edit user form for admin
    public ActionResult EditUser(int id)
    {
        if (Session["UserRole"] == null || Session["UserRole"].ToString() != "admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _context.Users.Find(id);
        if (user == null)
        {
            return HttpNotFound();
        }

        return View(user);
    }

    // ✅ Handle admin user updates
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditUser(User updatedUser, HttpPostedFileBase ProfileImage)
    {
        if (Session["UserRole"] == null || Session["UserRole"].ToString() != "admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _context.Users.Find(updatedUser.UserID);
        if (user == null)
        {
            return HttpNotFound();
        }

        // Update user details
        user.FullName = updatedUser.FullName;
        user.Phone = updatedUser.Phone;
        user.Address = updatedUser.Address;
        user.Role = updatedUser.Role;

        if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
        {
            user.PasswordHash = updatedUser.PasswordHash; // Hashing needed
        }

        //// Handle profile image upload
        //if (ProfileImage != null && ProfileImage.ContentLength > 0)
        //{
        //    string fileName = Path.GetFileName(ProfileImage.FileName);`
        //    string filePath = Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), fileName);
        //    ProfileImage.SaveAs(filePath);
        //    user.ProfilePicture = "/Content/ProfilePictures/" + fileName;
        //}

        _context.SaveChanges();

        TempData["SuccessMessage"] = "User profile updated successfully!";
        return RedirectToAction("ManageUsers");
    }
public ActionResult DeleteUser(int id)
    {
        using (var db = new EcommerceClothShopEntities())
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }
        return RedirectToAction("ManageUsers");
    }
}
