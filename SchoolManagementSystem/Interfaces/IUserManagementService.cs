using SchoolManagementSystem.Enums;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DataTransferObjects;
using SchoolManagementSystem.Models.QuerryResultDtos;
using AdminDto = SchoolManagementSystem.Models.DataTransferObjects.AdminDto;

namespace SchoolManagementSystem.Interfaces;

public interface IUserManagementService
{
    public Task<User?> FetchUser(int id);
    public Task<Student?> FetchStudent(int id);
    public Task<Teacher?> FetchTeacher(int id);
    public Task<Admin?> FetchAdmin(int id); 
    public Task<List<UserResultDto>> FetchGuests();
    public Task<Status> CreateStudent(StudentDto request);
    public Task<Status> CreateTeacher(TeacherDto request);
    public Task<Status> CreateAdmin(AdminDto request);
    public Task<Status> DeleteUser(int id);
    public Task<Status> CreateUser(UserDto request);
    public Task<User?> AttemptLogin(LoginUserDto request);
    Task<List<UserResultDto>> FetchTeachers();
}