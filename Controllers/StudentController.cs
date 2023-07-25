using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");

        [HttpGet(Name = "GetAllStudent")]
        public IEnumerable<Student> Get()
        {
            return db.GetStudents();
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public ActionResult<Student> Get(int id)
        {
            var student = db.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost(Name = "CreateStudent")]
        public IActionResult Create([FromBody] Student student)
        {
            if (ModelState.IsValid)
            {
                int newId = db.AddStudent(student);
                student.StudentId = newId;
                return CreatedAtRoute("GetStudent", new { id = newId }, student);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateStudent")]
        public IActionResult Put(int id, [FromBody] Student student)
        {
            if (student == null || student.StudentId != id)
            {
                return BadRequest();
            }

            var existingStudent = db.GetStudentById(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.StudentAdress = student.StudentAdress;
            existingStudent.StudentCity = student.StudentCity;
            existingStudent.StudentCountry = student.StudentCountry;
            existingStudent.StudentZipCode = student.StudentZipCode;

            db.UpdateStudent(existingStudent);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteStudent(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

