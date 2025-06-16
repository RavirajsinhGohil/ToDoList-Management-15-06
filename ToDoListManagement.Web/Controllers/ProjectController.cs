using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManagement.Entity.Constants;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Web.Controllers;

public class ProjectController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IProjectService _projectService;
    public ProjectController(IAuthService authService, IProjectService projectService) : base(authService)
    {
        _authService = authService;
        _projectService = projectService;
    }

    [HttpPost]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> AddProject(DashboardViewModel model)
    {
        int projectId = await _projectService.AddProject(model.Project, SessionUser.UserId);
        if (projectId > 0)
        {
            TempData["SuccessMessage"] = Constants.ProjectAddedMessage;
        }
        else if (projectId == -1)
        {
            TempData["ErrorMessage"] = Constants.ProjectAlreadyExistsMessage;
        }
        else
        {
            TempData["ErrorMessage"] = Constants.ProjectAddFailedMessage;
        }
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> GetProjectById(int projectId)
    {
        ProjectViewModel model = await _projectService.GetProjectByIdAsync(projectId);
        if (model == null)
        {
            return Json(new { success = false, message = Constants.ProjectNotFoundMessage });
        }
    
        return PartialView("~/Views/Dashboard/_UpdateProjectModal.cshtml", model);
    }

    [HttpPost]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateProject(ProjectViewModel model)
    {
        int? projectId = await _projectService.UpdateProjectAsync(model, SessionUser.UserId);
        if (projectId.HasValue && projectId.Value > 0)
        {
            TempData["SuccessMessage"] = Constants.ProjectUpdatedMessage;
        }
        else if (projectId == -1)
        {
            TempData["ErrorMessage"] = Constants.ProjectAlreadyExistsMessage;
        }
        else
        {
            TempData["ErrorMessage"] = Constants.ProjectUpdateFailedMessage;
        }
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> DeleteProject(int projectId)
    {
        bool isDeleted = await _projectService.DeleteProjectAsync(projectId, SessionUser.UserId);
        if (isDeleted)
        {
            TempData["SuccessMessage"] = Constants.ProjectDeletedMessage;
        }
        else
        {
            TempData["ErrorMessage"] = Constants.ProjectDeleteFailedMessage;
        }
        
        return RedirectToAction("Index", "Dashboard");
    }

}