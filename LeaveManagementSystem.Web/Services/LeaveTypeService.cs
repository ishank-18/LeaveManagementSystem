using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services;

public class LeaveTypeService(ApplicationDbContext context, IMapper mapper) : ILeaveTypeService
{
    public async Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync()
    {
        var data = await context.LeaveTypes.ToListAsync();
        var viewData = mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        return viewData;
    }

    public async Task<T?> GetLeaveType<T>(int id) where T : class
    {
        var leaveType = await context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (leaveType == null)
        {
            return null;
        }
        var viewData = mapper.Map<T>(leaveType);
        return viewData;
    }

    public async Task RemoveLeaveType(int id)
    {
        var leaveType = await context.LeaveTypes.FindAsync(id);
        if (leaveType != null)
        {
            context.LeaveTypes.Remove(leaveType);
            await context.SaveChangesAsync();
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateLeaveType(LeaveTypeCreateVM leaveTypeCreate)
    {
        var leaveType = mapper.Map<LeaveType>(leaveTypeCreate);
        context.Add(leaveType);
        await context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> EditLeaveType(LeaveTypeEditVM leaveTypeEdit)
    {
        var leaveType = mapper.Map<LeaveType>(leaveTypeEdit);
        try
        {
            context.Update(leaveType);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LeaveTypeExists(leaveTypeEdit.Id))
            {
                return (false, "Leave type not found.");
            }
            else
            {
                throw;
            }
        }
        return (true, null);
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
    {
        return await context.LeaveTypes.AnyAsync(lt => lt.Id != leaveTypeEdit.Id && lt.Name.ToLower().Equals(leaveTypeEdit.Name.ToLower()));
    }

    public async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        return await context.LeaveTypes.AnyAsync(lt => lt.Name.ToLower().Equals(name.ToLower()));
    }

    public bool LeaveTypeExists(int id)
    {
        return context.LeaveTypes.Any(e => e.Id == id);
    }
}
