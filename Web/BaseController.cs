using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web;

public abstract class BaseController : Controller
{
    public Guid? UserId => User.Identity?.IsAuthenticated == true ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : default;
}
