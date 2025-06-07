using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [Required]
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [Required]
        public int ProcedureId { get; set; }
        [ForeignKey("ProcedureId")]
        public Procedure Procedure { get; set; }

        [Required]
        [StringLength(50)]
        public string Room { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public string? Notes { get; set; }
    }
} 