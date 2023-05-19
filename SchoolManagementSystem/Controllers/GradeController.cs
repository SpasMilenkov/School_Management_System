using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Enums;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DataTransferObjects;
using SchoolManagementSystem.Services;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    [Authorize(Roles = "Admin")]
    public class GradeController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly IGradingService _gradingService;

        public GradeController(SchoolDbContext context, IGradingService gradingService)
        {
            _context = context;
            _gradingService = gradingService;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetGrades(int studentId)
        {
          if (_context.Teachers == null)
          {
              return NotFound();
          }
            var grade = await _gradingService.GetGRades(studentId);
            if(grade == null)
                return StatusCode(418); //¯\_(ツ)_/¯

            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<Grade>> AddGrade(GradeDto request)
        {
          if (_context.Grades == null)
              return Problem("Entity set 'SchoolDbContext.Grades'  is null.");
          
          var status = await _gradingService.AddGrade(request);
          
          if(status == Status.Fail)
              return StatusCode(418); //¯\_(ツ)_/¯

          return Ok("Grade added successfully");
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var status = await _gradingService.DeleteGrade(id);
            if(status == Status.Fail)
                return StatusCode(418); //¯\_(ツ)_/¯

            return Ok();
        }
    }
}