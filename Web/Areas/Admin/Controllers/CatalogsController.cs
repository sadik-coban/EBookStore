using Business.Features.Catalogs;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class CatalogsController(ICatalogsService catalogsService) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber,string? keywords)
    {
        if(pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<Catalog> list;
        if(pageNumber == null)
        {
            list = await catalogsService.GetCatalogListAsync(keywords, include: p => p.Include(p => p.User));
        }
        else
        {
            list = await catalogsService.GetCatalogListAsync(keywords, pageNumber: pageNumber!.Value, include: p => p.Include(p => p.User));
        }
        return View(list);
    }

    public IActionResult Create()
    {
        return View(new CatalogInputModel { Enabled = true});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CatalogInputModel catalog)
    {
        if (ModelState.IsValid)
        {
            var result = await catalogsService.CreateCatalogAsync(catalog,UserId!.Value);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu katalog zaten oluşturulmuş");
                return View(catalog);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(catalog);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var catalog = await catalogsService.GetCatalogById(id);
        return View(new CatalogInputModel { Name = catalog.Name, Enabled = catalog.Enabled});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,CatalogInputModel catalog)
    {
        if (ModelState.IsValid)
        {
            var result = await catalogsService.UpdateCatalogAsync(catalog,id);
            if (result < 0)
            {
                ModelState.AddModelError("", "Güncellemede hata ile karşılaşıldı");
                return View(catalog);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(catalog);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await catalogsService.DeleteCatalogAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
