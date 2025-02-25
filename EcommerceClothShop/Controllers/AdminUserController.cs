using EcommerceClothShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AdminUserController : Controller
{



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
    private EcommerceClothShopEntities db = new EcommerceClothShopEntities();

    public ActionResult ManageUsers(string search, string role)
    {
        var users = db.Users.AsQueryable();

        // 🔍 Filter by search (name or email)
        if (!string.IsNullOrEmpty(search))
        {
            users = users.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));
        }

        // 📂 Filter by role
        if (!string.IsNullOrEmpty(role))
        {
            users = users.Where(u => u.Role == role);
        }

        // Pass roles to the view for filtering
        ViewBag.Roles = db.Users.Select(u => u.Role).Distinct().ToList();

        return View(users.ToList());
    }

    [HttpPost]
    public ActionResult UpdateUserRoles(List<int> selectedUsers, string newRole)
    {
        if (selectedUsers != null && !string.IsNullOrEmpty(newRole))
        {
            var usersToUpdate = db.Users.Where(u => selectedUsers.Contains(u.UserID)).ToList();

            foreach (var user in usersToUpdate)
            {
                user.Role = newRole;
            }

            db.SaveChanges();
            TempData["SuccessMessage"] = "Selected users updated successfully!";
        }
        return RedirectToAction("ManageUsers");
    }
}
