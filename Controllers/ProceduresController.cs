using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Controllers
{
    public class ProceduresController : Controller
    {
        private readonly HospitalDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProceduresController(HospitalDbContext context, UserManager<User> userManager)
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

        // GET: Procedures
        public async Task<IActionResult> Index()
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View(await _context.Procedures.ToListAsync());
        }

        // GET: Procedures/Create
        public async Task<IActionResult> Create()
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        // POST: Procedures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Department,Duration")] Procedure procedure)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(procedure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(procedure);
        }

        // GET: Procedures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure == null)
            {
                return NotFound();
            }
            return View(procedure);
        }

        // POST: Procedures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Department,Duration")] Procedure procedure)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id != procedure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procedure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcedureExists(procedure.Id))
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
            return View(procedure);
        }

        // GET: Procedures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        // POST: Procedures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await SetUserRole();
            if (ViewBag.UserRole != UserRole.Admin)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure != null)
            {
                _context.Procedures.Remove(procedure);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedures.Any(e => e.Id == id);
        }
    }
} 