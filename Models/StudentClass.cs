using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionScolaire.Models
{
    public class StudentClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentClassId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int Year { get; set;}

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classe? Class { get; set; }

    }
}
