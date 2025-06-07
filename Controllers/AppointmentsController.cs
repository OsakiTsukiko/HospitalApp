using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HospitalDbContext _context;
        private readonly UserManager<User> _userManager;

        public AppointmentsController(HospitalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task SetUserRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.UserRole = user.Role;
            }
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Doctor)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var doctor = await _context.Doctors.FindAsync(currentUser.Id);

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Procedure)
                .Where(a => a.DoctorId == doctor.Id)
                .OrderBy(a => a.DateTime)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("GET Create action called");
            System.Diagnostics.Debug.WriteLine("GET Create action called");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Role != UserRole.Doctor)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Procedures = await _context.Procedures.ToListAsync();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            Console.WriteLine("POST Create action called");
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.Role != UserRole.Doctor)
            {
                System.Diagnostics.Debug.WriteLine("User is not a doctor or not logged in");
                return RedirectToAction("AccessDenied", "Home");
            }

            // Set the doctor ID before validation
            appointment.DoctorId = currentUser.Id;
            Console.WriteLine($"Setting DoctorId to: {appointment.DoctorId}");

            // Load navigation properties
            appointment.Doctor = await _context.Users
                .OfType<Doctor>()
                .FirstOrDefaultAsync(d => d.Id == appointment.DoctorId);
            
            appointment.Patient = await _context.Users
                .OfType<Patient>()
                .FirstOrDefaultAsync(p => p.Id == appointment.PatientId);
            
            appointment.Procedure = await _context.Procedures
                .FirstOrDefaultAsync(p => p.Id == appointment.ProcedureId);

            // Log all form data
            Console.WriteLine($"PatientId: {appointment.PatientId}");
            Console.WriteLine($"ProcedureId: {appointment.ProcedureId}");
            Console.WriteLine($"Room: {appointment.Room}");
            Console.WriteLine($"DateTime: {appointment.DateTime}");
            Console.WriteLine($"DoctorId: {appointment.DoctorId}");
            Console.WriteLine($"Notes: {appointment.Notes}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            // Log ModelState errors
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var modelStateEntry in ModelState)
                {
                    Console.WriteLine($"Key: {modelStateEntry.Key}");
                    foreach (var error in modelStateEntry.Value.Errors)
                    {
                        Console.WriteLine($"Error for {modelStateEntry.Key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.Patients = await _context.Patients.ToListAsync();
                ViewBag.Procedures = await _context.Procedures.ToListAsync();
                return View(appointment);
            }

            try
            {
                // Check if the room is available
                var existingAppointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Room == appointment.Room && 
                                            a.DateTime == appointment.DateTime);

                if (existingAppointment != null)
                {
                    System.Diagnostics.Debug.WriteLine("Room is already booked");
                    ModelState.AddModelError("Room", "This room is already booked at the selected time.");
                    ViewBag.Patients = await _context.Patients.ToListAsync();
                    ViewBag.Procedures = await _context.Procedures.ToListAsync();
                    return View(appointment);
                }

                // Add and save the appointment
                _context.Appointments.Add(appointment);
                System.Diagnostics.Debug.WriteLine("Appointment added to context");
                
                var result = await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"SaveChanges result: {result}");

                if (result > 0)
                {
                    TempData["Success"] = "Appointment scheduled successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SaveChanges returned 0 - no changes were saved");
                    ModelState.AddModelError("", "Failed to save the appointment. Please try again.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in Create: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while creating the appointment. Please try again.");
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Procedures = await _context.Procedures.ToListAsync();
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Doctor)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (appointment.DoctorId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Procedures = await _context.Procedures.ToListAsync();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,ProcedureId,Room,DateTime,Notes")] Appointment appointment)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Doctor)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id != appointment.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment.DoctorId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appointment.DoctorId = currentUser.Id;

                    // Check if the room is available at the specified time (excluding current appointment)
                    var isRoomAvailable = !await _context.Appointments
                        .AnyAsync(a => a.Room == appointment.Room && 
                                     a.DateTime == appointment.DateTime &&
                                     a.Id != appointment.Id);

                    if (!isRoomAvailable)
                    {
                        ModelState.AddModelError("Room", "This room is already booked at the selected time.");
                        ViewBag.Patients = await _context.Patients.ToListAsync();
                        ViewBag.Procedures = await _context.Procedures.ToListAsync();
                        return View(appointment);
                    }

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Appointment updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Procedures = await _context.Procedures.ToListAsync();
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Doctor)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Procedure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (appointment.DoctorId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (appointment.DoctorId != currentUser.Id)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Appointment deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/MyAppointments
        public async Task<IActionResult> MyAppointments()
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Patient)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var patient = await _context.Patients.FindAsync(currentUser.Id);

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Procedure)
                .Where(a => a.PatientId == patient.Id)
                .OrderBy(a => a.DateTime)
                .ToListAsync();

            return View(appointments);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        // GET: Appointments/GetUpcomingAppointments
        [HttpGet]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Json(new { appointments = new List<object>() });
            }

            var now = DateTime.Now;
            var twoDaysFromNow = now.AddHours(48);

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Procedure)
                .Where(a => 
                    (currentUser.Role == UserRole.Patient && a.PatientId == currentUser.Id) ||
                    (currentUser.Role == UserRole.Doctor && a.DoctorId == currentUser.Id))
                .Where(a => a.DateTime >= now && a.DateTime <= twoDaysFromNow)
                .OrderBy(a => a.DateTime)
                .Select(a => new
                {
                    id = a.Id,
                    dateTime = a.DateTime,
                    doctorName = a.Doctor.Name,
                    procedureName = a.Procedure.Name,
                    room = a.Room
                })
                .ToListAsync();

            return Json(new { appointments });
        }
    }
} 