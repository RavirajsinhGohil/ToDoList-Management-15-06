using ToDoListManagement.Entity.ViewModel;

namespace ToDoListManagement.Service.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync(int? userId);
}
