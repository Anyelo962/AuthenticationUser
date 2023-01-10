using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJWT.Controllers;

public class CountryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}