using Business.Features.Catalogs;
using Business.Features.Catalogs.Enums;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
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
            list = await catalogsService.GetCatalogListAsync(keywords,orderBy: OrderByCatalog.DateAscending);
        }
        else
        {
            list = await catalogsService.GetCatalogListAsync(keywords,orderBy: OrderByCatalog.DateAscending, pageNumber: pageNumber!.Value);
        }
        return View(list);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Catalog catalog)
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
        return View(await catalogsService.GetCatalogById(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Catalog catalog)
    {
        if (ModelState.IsValid)
        {
            var result = await catalogsService.UpdateCatalogAsync(catalog);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu katalog zaten oluşturulmuş");
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
