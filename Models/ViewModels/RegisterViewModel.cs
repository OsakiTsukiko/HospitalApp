using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string CNP { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        // Patient-specific fields
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

        // Doctor-specific fields
        [StringLength(100)]
        public string? Department { get; set; }

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }

        [StringLength(50)]
        public string? LicenseNumber { get; set; }
    }
} 