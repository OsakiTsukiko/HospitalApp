using HospitalManagement.Models;
using HospitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user;

                switch (model.Role)
                {
                    case UserRole.Patient:
                        user = new Patient
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            Name = model.Name,
                            CNP = model.CNP,
                            PhoneNumber = model.PhoneNumber,
                            BloodType = model.BloodType,
                            EmergencyContact = model.EmergencyContact,
                            Allergies = model.Allergies,
                            Weight = model.Weight,
                            Height = model.Height,
                            BirthDate = model.BirthDate,
                            Address = model.Address
                        };
                        break;
                    case UserRole.Doctor:
                        user = new Doctor
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            Name = model.Name,
                            CNP = model.CNP,
                            PhoneNumber = model.PhoneNumber,
                            Department = model.Department,
                            YearsOfExperience = model.YearsOfExperience,
                            LicenseNumber = model.LicenseNumber
                        };
                        break;
                    default:
                        user = new User
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            Name = model.Name,
                            CNP = model.CNP,
                            PhoneNumber = model.PhoneNumber,
                            Role = model.Role
                        };
                        break;
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
} 