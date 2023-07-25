using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionScolaire.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string? StudentAdress { get; set; }

        [Required]
        [StringLength(50)]
        public string? StudentCity { get; set; }

        [Required]
        [StringLength(50)]
        public string? StudentZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string? StudentCountry { get; set; }
    }
}
