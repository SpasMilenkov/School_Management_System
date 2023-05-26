using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Enums;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DataTransferObjects;

namespace SchoolManagementSystem.Services.DataManipulation;

public class GradingService : IGradingService
{
    private readonly SchoolDbContext _context;
    public GradingService(SchoolDbContext context)
    {
        _context = context;
    }
    public async Task<Status> AddGrade(GradeDto request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        var teacher = await _context.Teachers.FindAsync(request.TeacherId);
        if (student == null || teacher == null)
            return Status.Fail;
        var grade = new Grade()
        {
            Value = request.Value,
            Grader = teacher,
            Owner = student,
            Date = DateTime.Now,
        };
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        return Status.Success;
    }
    
    public async Task<Status> DeleteGrade(int gradeId)
    {
        var grade = await _context.Grades.FindAsync(gradeId);
        if (grade == null)
            return Status.Fail;
        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
        return Status.Success;
    }

    public async Task<Status> UpdateGrade(int gradeId, GradeDto request)
    {
        var grade = await _context.Grades.FindAsync(gradeId);
        if (grade == null)
            return Status.Fail;
        
        return Status.Success;
    }

    public async Task<List<Grade>> GetGrades(int requestedId)
    {
        return await _context.Grades.Where(g => g.Owner.StudentId == requestedId).ToListAsync();
    }
}