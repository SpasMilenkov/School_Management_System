using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Enums;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DataTransferObjects;

namespace SchoolManagementSystem.Services.DataManipulation;

public class AbsenceService : IAbsenceService
{
    private readonly SchoolDbContext _context;
    
    public AbsenceService(SchoolDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Gets an absence by its id
    /// </summary>
    /// <param name="absenceId">The id of the absence</param>
    /// <returns>Absence or null</returns>
    public async Task<Absence?> GetAbsenceById(int absenceId) => await _context.Absences.FindAsync(absenceId);
    
    /// <summary>
    /// Gets the absence of a student using the userId
    /// </summary>
    /// <param name="studentId">The id of the student</param>
    /// <returns>A list of absence objects</returns>
    public async Task<List<Absence>> GetAbsenceByStudent(int studentId)
    {
        return await  _context.Absences
            .Include(absence => absence.Teacher)
            .Include(absence => absence.Subject)
            .Include(absence =>  absence.Student)
            .Where(a => a.Student.StudentId == studentId).ToListAsync();
    }
    /// <summary>
    /// Gets all the absences a teacher has put into the system
    /// </summary>
    /// <param name="teacherId">The id of the teacher</param>
    /// <returns>A list of absence objects</returns>
    public async Task<List<Absence>> GetAbsenceByTeacher(int teacherId)
    {
        return await _context.Absences.Where(a => a.Teacher.TeacherId == teacherId).ToListAsync();
    }
    /// <summary>
    /// Removes an absence entry from the db
    /// </summary>
    /// <param name="absenceId">The id of the absence</param>
    /// <returns>Status of the operation</returns>
    public async Task<Status> RemoveAbsenceById(int absenceId)
    {
        var absence = await GetAbsenceById(absenceId);
        if (absence == null)
            return Status.Fail;
        _context.Absences.Remove(absence);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            return Status.Fail;
        }
        return Status.Success;
    }
    /// <summary>
    /// Changes the state of an absence entry from 'unexcused' to 'excused'
    /// </summary>
    /// <param name="absenceId">Id of the absence</param>
    /// <returns>Status of the operation</returns>
    public async Task<Status> ExcuseAbsenceById(int absenceId)
    {
        var absence = await _context.Absences.FindAsync(absenceId);
        if (absence == null)
            return Status.Fail;

        absence.Excused = true;
        await _context.SaveChangesAsync();
        return Status.Success;
    }
    /// <summary>
    /// Changes the state of multiple absence entries from 'unexcused' to 'excused'
    /// </summary>
    /// <param name="studentId">Id of the student whose absence are being excused</param>
    /// <param name="subjectId"> Id of the teacher who is excusing the absence</param>
    /// <param name="date">Date when the absence was excused </param> 
    /// <returns>Status of the operation</returns>
    public async Task<Status> ExcuseAbsences(int studentId, int subjectId, DateTime date)
    {
        var absences = await _context.Absences.Where(a => a.Student.StudentId == studentId 
                                                          && a.Subject.Id == subjectId 
                                                          && a.Date == date).ToListAsync();
        foreach (var absence in absences)
            absence.Excused = true;
            
        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            return Status.Fail;
        }
        return Status.Success;
    }
    /// <summary>
    /// Adds an absence entry to the database
    /// </summary>
    /// <param name="request">AbsenceDto with entry data</param>
    /// <returns>Status of the operation</returns>
    public async Task<Status> CreateAbsence(AbsenceDto request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        var teacher = await _context.Teachers.FindAsync(request.TeacherId);
        var subject = await _context.Subjects.FindAsync(request.SubjectId);
        if (student == null || teacher == null || subject == null)
            return Status.Fail;

        var absence = new Absence
        {
            Teacher = teacher,
            Student = student,
            Subject = subject,
            Date = DateTime.Today,
            Excused = false
        };
        try
        {
            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Status.Fail;
        }
        return Status.Success;
    }
    
}