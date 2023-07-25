using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");

        [HttpGet(Name = "GetAllStudentClass")]
        public IEnumerable<StudentClass> Get()
        {
            return db.GetStudentClass();
        }

        [HttpGet("{id}", Name = "GetStudentClass")]
        public ActionResult<StudentClass> Get(int id)
        {
            var studentClass = db.GetStudentClassById(id);
            if (studentClass == null)
            {
                return NotFound();
            }
            return studentClass;
        }

        [HttpPost(Name = "CreateStudentClass")]
        public IActionResult Create([FromBody] StudentClass studentClass)
        {
            if (ModelState.IsValid)
            {
                int newId = db.AddStudentClass(studentClass);
                studentClass.StudentClassId = newId;
                return CreatedAtRoute("GetStudentClass", new { id = newId }, studentClass);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateStudentClass")]
        public IActionResult Put(int id, [FromBody] StudentClass studentClass)
        {
            if (studentClass == null || studentClass.StudentClassId != id)
            {
                return BadRequest();
            }

            var existingStudentClass = db.GetStudentClassById(id);
            if (existingStudentClass == null)
            {
                return NotFound();
            }

            existingStudentClass.ClassId = studentClass.ClassId;
            existingStudentClass.StudentId = studentClass.StudentId;
            existingStudentClass.Year = studentClass.Year;


            db.UpdateStudentClass(existingStudentClass);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudentClass")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteStudentClass(id))
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
