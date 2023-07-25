using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseNoteController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");
        [HttpGet(Name = "GetAllCourseNote")]
        public IEnumerable<CourseNote> Get()
        {
            return db.GetCourseNote();
        }

        [HttpGet("{id}", Name = "GetCourseNote")]
        public ActionResult<CourseNote> Get(int id)
        {
            var courseNote = db.GetCourseNoteById(id);
            if (courseNote == null)
            {
                return NotFound();
            }
            return courseNote;
        }

        [HttpPost(Name = "CreateCourseNote")]
        public IActionResult Create([FromBody] CourseNote courseNote)
        {
            if (ModelState.IsValid)
            {
                int newId = db.AddCourseNote(courseNote);
                courseNote.CourseNoteId = newId;
                return CreatedAtRoute("GetCourseNote", new { id = newId }, courseNote);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateCourseNote")]
        public IActionResult Put(int id, [FromBody] CourseNote courseNote)
        {
            if (courseNote == null || courseNote.CourseNoteId != id)
            {
                return BadRequest();
            }

            var existingCourseNote = db.GetCourseNoteById(id);
            if (existingCourseNote == null)
            {
                return NotFound();
            }

            existingCourseNote.Mark = courseNote.Mark;
            existingCourseNote.StudentId = courseNote.StudentId;
            existingCourseNote.SubjectId = courseNote.SubjectId;


            db.UpdateCourseNote(existingCourseNote);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteCourseNote")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteCourseNote(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

      
        [HttpGet("{studentId}/{subjectId}", Name = "GetStudentSubjetNote")]
        public ActionResult<List<CourseNote>> GetStudentSubjetNote(int studentId, int subjectId)
        {
            var courseNote = db.GetStudentSubjectNote(subjectId, studentId);
            if (courseNote == null)
            {
                return NotFound();
            }
            return courseNote;
        }
    }
}
