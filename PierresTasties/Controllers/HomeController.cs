using Microsoft.AspNetCore.Mvc;
using PierresTasties.Models;
using System.Diagnostics;

namespace PierresTasties.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

}
