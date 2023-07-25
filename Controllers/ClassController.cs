using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");

        [HttpGet(Name = "GetAllClass")]
        public IEnumerable<Classe> Get()
        {
            return db.GetClasses();
        }

        //[HttpGet("{schoolId}", Name = "GetAllSchoolClass")]
        //public IEnumerable<Class> Get(int schoolId)
        //{
        //    return db.GetClasses();
        //}

        [HttpGet("{id}", Name = "GetClass")]
        public ActionResult<Classe> Get(int id)
        {
            var lass = db.GetClassById(id);
            if (lass == null)
            {
                return NotFound();
            }
            return lass;
        }

        [HttpPost(Name = "CreateClass")]
        public IActionResult Create([FromBody] Classe lass)
        {
            if (ModelState.IsValid)
            {
                int newId = db.AddClass(lass);
                lass.ClassId = newId;
                return CreatedAtRoute("GetClass", new { id = newId }, lass);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateClass")]
        public IActionResult Put(int id, [FromBody] Classe lass)
        {
            if (lass == null || lass.ClassId != id)
            {
                return BadRequest();
            }

            var existingClass = db.GetClassById(id);
            if (existingClass == null)
            {
                return NotFound();
            }

            existingClass.ClassName = lass.ClassName;
            existingClass.SchoolId = lass.SchoolId;
           

            db.UpdateClass(existingClass);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteClass")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteClass(id))
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
