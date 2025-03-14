using EcommerceClothShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AdminProductController : Controller
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

    // ✅ Manage Products
    public ActionResult ManageProducts()
    {
        var products = _context.Products.Where(p => !p.IsDeleted).ToList(); 
        return View(products);
    }

    public ActionResult CreateProduct()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateProduct(Product product, HttpPostedFileBase ProductImage)
    {
        if (ModelState.IsValid)
        {
            // Handle product image upload
            if (ProductImage != null && ProductImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ProductImage.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Content/ProductImages/"), fileName);
                ProductImage.SaveAs(filePath);
                product.ImageURL = "/Content/ProductImages/" + fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("ManageProducts");
        }

        // Reload categories if validation fails
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
        return View(product);
    }

    public ActionResult EditProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return HttpNotFound();
        }

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditProduct(Product updatedProduct, HttpPostedFileBase ProductImage)
    {
        var product = _context.Products.Find(updatedProduct.ProductID);
        if (product == null)
        {
            return HttpNotFound();
        }

        // Update product details
        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.Stock = updatedProduct.Stock;
        product.CategoryID = updatedProduct.CategoryID;

        // Handle product image update
        if (ProductImage != null && ProductImage.ContentLength > 0)
        {
            string fileName = Path.GetFileName(ProductImage.FileName);
            string filePath = Path.Combine(Server.MapPath("~/Content/ProductImages/"), fileName);
            ProductImage.SaveAs(filePath);
            product.ImageURL = "/Content/ProductImages/" + fileName;
        }

        _context.SaveChanges();
        TempData["SuccessMessage"] = "Product updated successfully!";
        return RedirectToAction("ManageProducts");
    }



    // POST: Permanent Delete

    [HttpPost]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            product.IsDeleted = true; // Mark as deleted
            _context.SaveChanges();
        }

        TempData["SuccessMessage"] = "Product marked as deleted!";
        return RedirectToAction("ManageProducts");
    }



}
