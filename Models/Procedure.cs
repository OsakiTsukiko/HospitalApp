using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public enum Department
    {
        Cardiology,
        Neurology,
        Orthopedics,
        Pediatrics,
        Surgery,
        [Display(Name = "Internal Medicine")]
        InternalMedicine,
        Emergency,
        Radiology
    }

    public class Procedure
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }

        [Required]
        public int Duration { get; set; } // Duration in minutes
    }
} 