using GestionScolaire.DatabaseClasses;
using GestionScolaire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionScolaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Gestion_Scolaire.db");

        [HttpGet(Name = "GetAllSchool")]
        public IEnumerable<School> Get()
        {
            return db.GetSchools();
        }

        [HttpGet("{id}", Name = "GetSchool")]
        public ActionResult<School> Get(int id)
        {
            var Book = db.GetSchoolById(id);
            if (Book == null)
            {
                return NotFound();
            }
            return Book;
        }

        [HttpPost(Name = "CreateSchool")]
        public IActionResult Create([FromBody] School School)
        {
            if (ModelState.IsValid)
            {
                int newSchoolId = db.AddSchool(School);
                School.SchoolId = newSchoolId;
                return CreatedAtRoute("GetSchool", new { id = newSchoolId }, School);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateSchool")]
        public IActionResult Put(int id, [FromBody] School school)
        {
            if (school == null || school.SchoolId != id)
            {
                return BadRequest();
            }

            var existingSchool = db.GetSchoolById(id);
            if (existingSchool == null)
            {
                return NotFound();
            }

            existingSchool.SchoolName = school.SchoolName;
            existingSchool.SchoolCity = school.SchoolCity;
            existingSchool.SchoolAdress = school.SchoolAdress;
            existingSchool.SchoolCountry = school.SchoolCountry;
            existingSchool.SchoolZipCode = school.SchoolZipCode;

            db.UpdateSchool(existingSchool);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteSchool")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteSchool(id))
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
