using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.WebApi.Data;
using StudentManagement.WebApi.Data.Entities;

namespace StudentManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagementDbContext _dbContext;

        public StudentsController(StudentManagementDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _dbContext.Students.AsNoTracking().ToListAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchStudent([FromRoute] int id)
        {
            var student = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if(student is null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromForm] StudentEntity student)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int maxNo = _dbContext.Students.Max(s => (int?)s.No) ?? 0;
            student.No = maxNo + 1;

            var newStudent = new StudentEntity();

            newStudent.No = student.No;
            newStudent.Name = student.Name;
            newStudent.Surname = student.Surname;
            newStudent.Class = student.Class;

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(SearchStudent), new { student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromForm] StudentEntity student )
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbStudent = _dbContext.Students.FirstOrDefault(s => s.Id == id);

            if(dbStudent is null)
            {
                return NotFound();
            }

            bool noExistsForAnotherStudent =_dbContext.Students.Any(s => s.No == student.No && s.Id != id);

            if(noExistsForAnotherStudent)
            {
                dbStudent.No = GetUniqueStudentNumber();
            }
            else
            {
                dbStudent.No = student.No;
            }

            dbStudent.Name = student.Name;
            dbStudent.Surname = student.Surname;
            dbStudent.Class = student.Class;

            await _dbContext.SaveChangesAsync();

            return Ok(dbStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var dbStudent = _dbContext.Students.FirstOrDefault(s => s.Id == id);

            if(dbStudent is null)
            {
                return NotFound();
            }

            _dbContext.Students.Remove(dbStudent);
            await _dbContext.SaveChangesAsync();

            return Ok(dbStudent);
        }


        [NonAction]
        private int GetUniqueStudentNumber()
        {
            var existingNos = _dbContext.Students.Select(s => s.No).ToHashSet();

            int uniqueNo = 1;

            while (existingNos.Contains(uniqueNo))
            {
                uniqueNo++;
            }

            return uniqueNo;
        }
    }
}
