using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJWT.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}