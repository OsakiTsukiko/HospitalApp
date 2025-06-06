using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Patient : User
    {
        [StringLength(5)]
        public string? BloodType { get; set; }

        [StringLength(100)]
        public string? EmergencyContact { get; set; }

        [StringLength(500)]
        public string? Allergies { get; set; }

        [Range(0, 500)]
        public double? Weight { get; set; }

        [Range(0, 300)]
        public double? Height { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public Patient()
        {
            Role = UserRole.Patient;
        }
    }
} 