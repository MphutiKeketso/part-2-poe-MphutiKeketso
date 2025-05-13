using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewData["Message"] = "EventEase Booking System - Your trusted event booking platform.";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Contact us for support and inquiries.";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
