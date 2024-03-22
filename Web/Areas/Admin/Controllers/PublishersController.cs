using Business.Features.Publishers;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class PublishersController(IPublishersService publishersService) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber, string? keywords)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<Publisher> list;
        if (pageNumber == null)
        {
            list = await publishersService.GetPublisherListAsync(keywords, include: p => p.Include(p => p.User));
        }
        else
        {
            list = await publishersService.GetPublisherListAsync(keywords, pageNumber: pageNumber!.Value, include: p => p.Include(p => p.User));
        }
        return View(list);
    }

    public IActionResult Create()
    {
        return View(new PublisherInputModel { Enabled = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PublisherInputModel publisher)
    {
        if (ModelState.IsValid)
        {
            var result = await publishersService.CreatePublisherAsync(publisher, UserId!.Value);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu yayınevi zaten oluşturulmuş");
                return View(publisher);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(publisher);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var catalog = await publishersService.GetPublisherById(id);
        return View(new PublisherInputModel { Name = catalog.Name, Enabled = catalog.Enabled });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PublisherInputModel publisher)
    {
        if (ModelState.IsValid)
        {
            var result = await publishersService.UpdatePublisherAsync(publisher, id);
            if (result < 0)
            {
                ModelState.AddModelError("", "Güncellemede hata ile karşılaşıldı");
                return View(publisher);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(publisher);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await publishersService.DeletePublisherAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
