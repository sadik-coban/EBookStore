using Business.Features.Authors;
using Business.Features.Catalogs;
using Business.Features.Products;
using Business.Features.Publishers;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Policy;
using X.PagedList;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class ProductsController
    (
        IProductsService productsService,
        ICatalogsService catalogsService,
        IAuthorsService authorsService,
        IPublishersService publishersService
    ) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber, string? keywords)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<Product> list;
        if (pageNumber == null)
        {
            list = await productsService.GetProductListAsync(keywords, include: query => query.Include(p => p.Catalogs).Include(p => p.Authors).Include(p => p.Publisher).Include(p => p.User));
        }
        else
        {
            list = await productsService.GetProductListAsync(keywords, pageNumber: pageNumber!.Value, include: query => query.Include(p => p.Catalogs).Include(p => p.Authors).Include(p => p.Publisher).Include(p => p.User));
        }
        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View(new ProductInputModel { Enabled = true, DiscountRate = "0", Price = "0" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductInputModel product)
    {
        await PopulateDropdowns();
        //if (ModelState.IsValid)
        //{
            var result = await productsService.CreateProductAsync(product, UserId!.Value);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu ürün zaten oluşturulmuş");
                return View(product);
            }
            return RedirectToAction(nameof(Index));
        //}
        //return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        await PopulateDropdowns();
        var product = await productsService.GetProductById(id, include: query => query.Include(p => p.Catalogs).Include(p => p.Authors).Include(p => p.Publisher));
        return View(new ProductInputModel 
        { 
            Name = product.Name, 
            Enabled = product.Enabled, 
            DiscountRate = product.DiscountRate.ToString(),  
            Description = product.Description,
            Price = product.Price.ToString("f2", CultureInfo.CreateSpecificCulture("tr-TR")),
            Catalogs = product.Catalogs.Select(p => p.Id).ToList(),
            Authors = product.Authors.Select(p => p.Id).ToList(),
            Publisher = product.PublisherId,
            OriginalImage = product.Image
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductInputModel product)
    {
        await PopulateDropdowns();
        if (ModelState.IsValid)
        {
            var result = await productsService.UpdateProductAsync(product, id);
            if (result < 0)
            {
                ModelState.AddModelError("", "Güncellemede hata ile karşılaşıldı");
                return View(product);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await productsService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        ViewBag.Catalogs = (await catalogsService.GetKeyNameListAsync()).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        ViewBag.Publishers = (await publishersService.GetKeyNameListAsync()).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        ViewBag.Authors = (await authorsService.GetKeyNameListAsync()).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
    }
}
