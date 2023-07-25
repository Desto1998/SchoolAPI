using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionScolaire.Models
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolId { get; set; }

        [Required]
        [StringLength(50)]
        public string? SchoolName { get; set; }

        [Required]
        [StringLength(50)]
        public string? SchoolAdress { get; set; }

        [Required]
        [StringLength(50)]
        public string? SchoolCity { get; set; }

        [Required]
        [StringLength(50)]
        public string? SchoolZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string? SchoolCountry { get; set; }
    }
}
