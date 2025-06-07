using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        public int Duration { get; set; } // Duration in minutes
    }
} 