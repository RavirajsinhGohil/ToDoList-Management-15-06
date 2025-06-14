using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Web.Controllers;

[Authorize]
public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IAuthService authService, IDashboardService dashboardService)
        : base(authService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ViewBag.SessionUser == null || string.IsNullOrEmpty(ViewBag.SessionUser.Email))
        {
            return RedirectToAction("Login", "Auth");
        }
        DashboardViewModel model = await _dashboardService.GetDashboardDataAsync(ViewBag.SessionUser.UserId);
        return View(model);
    }
}
