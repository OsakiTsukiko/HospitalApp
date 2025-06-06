using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Doctor : User
    {
        [StringLength(100)]
        public string? Department { get; set; }

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }

        [StringLength(50)]
        public string? LicenseNumber { get; set; }

        public Doctor()
        {
            Role = UserRole.Doctor;
        }
    }
} 