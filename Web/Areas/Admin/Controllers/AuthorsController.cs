using Business.Features.Authors;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class AuthorsController(IAuthorsService authorsService) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber, string? keywords)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<Author> list;
        if (pageNumber == null)
        {
            list = await authorsService.GetAuthorListAsync(keywords,include: p=> p.Include(p => p.User));
        }
        else
        {
            list = await authorsService.GetAuthorListAsync(keywords, pageNumber: pageNumber!.Value, include: p => p.Include(p => p.User));
        }
        return View(list);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorInputModel author)
    {
        if (ModelState.IsValid)
        {
            var result = await authorsService.CreateAuthorAsync(author, UserId!.Value);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu yazar zaten oluşturulmuş");
                return View(author);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var catalog = await authorsService.GetAuthorById(id);
        return View(new AuthorInputModel { Name = catalog.Name, Enabled = catalog.Enabled });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AuthorInputModel author)
    {
        if (ModelState.IsValid)
        {
            var result = await authorsService.UpdateAuthorAsync(author, id);
            if (result < 0)
            {
                ModelState.AddModelError("", "Güncellemede hata ile karşılaşıldı");
                return View(author);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await authorsService.DeleteAuthorAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
