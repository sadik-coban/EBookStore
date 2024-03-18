using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
