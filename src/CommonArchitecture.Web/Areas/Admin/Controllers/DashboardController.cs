using CommonArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AuthorizeUser]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

