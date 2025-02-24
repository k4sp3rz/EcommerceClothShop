using EcommerceClothShop.Models;
using System.Linq;
using System.Web.Mvc;

public class AdminCateController : Controller
{
    private readonly EcommerceClothShopEntities _context = new EcommerceClothShopEntities();

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (Session["UserRole"] == null || Session["UserRole"].ToString() != "admin")
        {
            filterContext.Result = new RedirectResult("~/Auth/Login");
        }
        base.OnActionExecuting(filterContext);
    }

    // ✅ Show all categories
    public ActionResult ManageCategories()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // ✅ Show create category form
    public ActionResult CreateCategory()
    {
        return View();
    }

    // ✅ Handle create category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Category added successfully!";
            return RedirectToAction("ManageCategories");
        }
        return View(category);
    }

    // ✅ Show edit category form
    public ActionResult EditCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return HttpNotFound();
        }
        return View(category);
    }

    // ✅ Handle edit category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditCategory(Category updatedCategory)
    {
        var category = _context.Categories.Find(updatedCategory.CategoryID);
        if (category == null)
        {
            return HttpNotFound();
        }

        category.CategoryName = updatedCategory.CategoryName;

        _context.SaveChanges();
        TempData["SuccessMessage"] = "Category updated successfully!";
        return RedirectToAction("ManageCategories");
    }

    // ✅ Delete a category
    public ActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Category deleted successfully!";
        }
        return RedirectToAction("ManageCategories");
    }
}
