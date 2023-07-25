using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");

        [HttpGet(Name = "GetAllSubject")]
        public IEnumerable<Subject> Get()
        {
            return db.GetSubjects();
        }

        [HttpGet("{id}", Name = "GetSubject")]
        public ActionResult<Subject> Get(int id)
        {
            var subject = db.GetSubjectById(id);
            if (subject == null)
            {
                return NotFound();
            }
            return subject;
        }

        [HttpPost(Name = "CreateSubject")]
        public IActionResult Create([FromBody] Subject subject)
        {
            if (ModelState.IsValid)
            {
                int newId = db.AddSubject(subject);
                subject.SubjectId = newId;
                return CreatedAtRoute("GetSubject", new { id = newId }, subject);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateSubject")]
        public IActionResult Put(int id, [FromBody] Subject subject)
        {
            if (subject == null || subject.SubjectId != id)
            {
                return BadRequest();
            }

            var existingSubject = db.GetSubjectById(id);
            if (existingSubject == null)
            {
                return NotFound();
            }

            existingSubject.SubjectName = subject.SubjectName;
            existingSubject.MaxPoint = subject.MaxPoint;
            existingSubject.ClassId = subject.ClassId;


            db.UpdateSubject(existingSubject);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteSubject")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteSubject(id))
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
