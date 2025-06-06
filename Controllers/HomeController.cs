using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Models;
using HospitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly HospitalDbContext _context;

    public HomeController(UserManager<User> userManager, HospitalDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.UserRole = user.Role;
        ViewBag.UserName = user.Name;

        if (user.Role == UserRole.Admin)
        {
            var allUsers = await _context.Users.ToListAsync();
            return View("AdminDashboard", allUsers);
        }

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new RegisterViewModel
        {
            Email = user.Email ?? "",
            Name = user.Name,
            CNP = user.CNP,
            PhoneNumber = user.PhoneNumber ?? "",
            Role = user.Role
        };

        if (user is Patient patient)
        {
            model.BloodType = patient.BloodType;
            model.EmergencyContact = patient.EmergencyContact;
            model.Allergies = patient.Allergies;
            model.Weight = patient.Weight;
            model.Height = patient.Height;
            model.BirthDate = patient.BirthDate;
            model.Address = patient.Address;
        }
        else if (user is Doctor doctor)
        {
            model.Department = doctor.Department;
            model.YearsOfExperience = doctor.YearsOfExperience;
            model.LicenseNumber = doctor.LicenseNumber;
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(RegisterViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Update common properties
        user.Name = model.Name;
        user.CNP = model.CNP;
        user.PhoneNumber = model.PhoneNumber;

        if (user is Patient patient)
        {
            patient.BloodType = model.BloodType;
            patient.EmergencyContact = model.EmergencyContact;
            patient.Allergies = model.Allergies;
            patient.Weight = model.Weight;
            patient.Height = model.Height;
            patient.BirthDate = model.BirthDate;
            patient.Address = model.Address;
        }
        else if (user is Doctor doctor)
        {
            doctor.Department = model.Department;
            doctor.YearsOfExperience = model.YearsOfExperience;
            doctor.LicenseNumber = model.LicenseNumber;
        }

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Dashboard");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser?.Role != UserRole.Admin)
        {
            return Forbid();
        }

        var userToDelete = await _userManager.FindByIdAsync(userId);
        if (userToDelete != null && userToDelete.Role != UserRole.Admin)
        {
            await _userManager.DeleteAsync(userToDelete);
            TempData["Success"] = "User deleted successfully!";
        }

        return RedirectToAction("Dashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
