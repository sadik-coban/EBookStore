using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]

public class DashboardsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
