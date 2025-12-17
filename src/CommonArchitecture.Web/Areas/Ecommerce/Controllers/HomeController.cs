using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.Web.Areas.Ecommerce.Controllers;

[Area("Ecommerce")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Shop");
    }
}

