using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services
{
    public interface ILeaveTypeService
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
        Task<(bool Succeeded, string? ErrorMessage)> CreateLeaveType(LeaveTypeCreateVM leaveTypeCreate);
        Task<(bool Succeeded, string? ErrorMessage)> EditLeaveType(LeaveTypeEditVM leaveTypeEdit);
        Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync();
        Task<T?> GetLeaveType<T>(int id) where T : class;
        bool LeaveTypeExists(int id);
        Task RemoveLeaveType(int id);
    }
}