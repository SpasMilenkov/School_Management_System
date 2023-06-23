using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models.QuerryResultDtos;

namespace SchoolManagementSystem.Services.DataOrganization;

public class AdminDataBundler: IAdminDataBundler
{
    private readonly IUserManagementService _userManagementService;
    public AdminDataBundler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }
    public async Task<List<UserResultDto>> FetchAllStudents()
    {
        var students = await _userManagementService.FetchAllStudents();
        var result = students.Select(student => new UserResultDto()
        {
            Id = student.StudentId,
            Name = student.User.Name,
            Role = student.User.Role,
            Email = student.User.Email,
            Attributes = new Dictionary<string, string>
            {
                { "Faculty", student.Faculty.Name },
                { "Specialty", student.Specialty.Name },
                { "Course", student.Course.ToString() }
            },
        }).ToList();
        return result;
    }

    public async Task<List<UserResultDto>> FetchAllTeachers()
    {
        var teachers = await _userManagementService.FetchAllTeachers();
        var result  = teachers.Select(teacher => new UserResultDto()
        {
            Id = teacher.TeacherId,
            Name = teacher.User.Name,
            Role = teacher.User.Role,
            Attributes = new Dictionary<string, string>
            {
                { "Title", teacher.Title }
            },
            Email = teacher.User.Email
        }).ToList();
        return result;
    }

    public async Task<List<UserResultDto>> FetchAllAdmins()
    {
        var admins = await _userManagementService.FetchAllAdmins();
        var result = admins.Select(admin => new UserResultDto
            {
                Id = admin.AdminId,
                Name = admin.User.Name,
                Email = admin.User.Email,
                Role = admin.User.Role,
            }).ToList();
        return result;
    }
}