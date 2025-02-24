using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceClothShop.Models;

public class UserController : Controller
{
    private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

    // ✅ Show user details
    public ActionResult Details()
    {
        if (Session["UserEmail"] == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        string userEmail = Session["UserEmail"].ToString();
        var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
        {
            return HttpNotFound();
        }

        return View(user);
    }

    // ✅ Show edit profile form
    public ActionResult Edit()
    {
        if (Session["UserEmail"] == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        string userEmail = Session["UserEmail"].ToString();
        var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
        {
            return HttpNotFound();
        }

        return View(user);
    }

    // ✅ Handle profile updates
    [HttpPost]
    public ActionResult Edit(User updatedUser, HttpPostedFileBase ProfileImage)
    {
        if (Session["UserEmail"] == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == updatedUser.Email);
        if (user == null)
        {
            return HttpNotFound();
        }

        // Update user details
        user.FullName = updatedUser.FullName;
        user.Phone = updatedUser.Phone;
        user.Address = updatedUser.Address;

        if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
        {
            user.PasswordHash = updatedUser.PasswordHash; // Hashing needed
        }

        // Handle profile image upload
        //if (ProfileImage != null && ProfileImage.ContentLength > 0)
        //{
        //    string fileName = Path.GetFileName(ProfileImage.FileName);
        //    string filePath = Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), fileName);
        //    ProfileImage.SaveAs(filePath);
        //    user.ProfilePicture = "/Content/ProfilePictures/" + fileName;
        //}

        _context.SaveChanges();

        // Update session if name changed
        Session["UserName"] = user.FullName;

        TempData["SuccessMessage"] = "Profile updated successfully!";
        return RedirectToAction("Details");
    }
}
