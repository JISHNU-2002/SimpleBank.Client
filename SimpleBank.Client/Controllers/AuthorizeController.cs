using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;
using System.Security.Claims;

namespace SimpleBank.Client.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public AuthorizeController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _client.PostAsync<Result<UserResponse>>(ApiConstants.AuthorizeUser, userRequest);
                    if (result.IsError)
                    {
                        _toastNotification.AddWarningToastMessage("Invalid login credentials");
                        return View();
                    }

                    if (!result.IsError)
                    {
                        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userRequest.Username));

                        claimsIdentity.AddClaim(new Claim("AccountNumber", result.Response!.AccountNumber));
                        claimsIdentity.AddClaim(new Claim("FormId", result.Response.FormId.ToString()));

                        foreach (var role in result.Response!.roles)
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                        }

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties
                            {
                                IsPersistent = false,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                            }
                        );

                        if (result.Response!.roles.Any(r => r.RoleName == "SuperAdmin"))
                        {
                            _toastNotification.AddSuccessToastMessage("Welcome to Admin Dashboard");
                            return RedirectToAction("SuperAdminDashboard", "AdminDashboard");
                        }
                        else if (result.Response!.roles.Any(r => r.RoleName == "Manager"))
                        {
                            _toastNotification.AddSuccessToastMessage("Welcome to Manager Dashboard");
                            return RedirectToAction("ManagerDashboard", "AdminDashboard");
                        }
                        else if (result.Response!.roles.Any(r => r.RoleName == "Customer"))
                        {
                            _toastNotification.AddSuccessToastMessage("Login success");
                            return RedirectToAction("Dashboard", "Account");
                        }                            
                    }

                    foreach (var error in result!.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage!);
                    }
                }
                catch(Exception ex)
                {
                    _toastNotification.AddErrorToastMessage(ex.Message);
                    return View();
                }       
            }

            _toastNotification.AddWarningToastMessage("Invalid login credentials");
            return View(userRequest);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _client.PostAsync<Result<UserResponse>>(ApiConstants.RegisterUser, registerRequest);

                    if (!result.IsError)
                    {
                        _toastNotification.AddSuccessToastMessage("Registration success");
                        return RedirectToAction("Users", "AdminDashboard");
                    }

                    foreach (var error in result!.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage!);
                    }
                }
                catch(Exception ex)
                {
                    _toastNotification.AddErrorToastMessage(ex.Message);
                    return View();
                }
            }

            _toastNotification.AddWarningToastMessage("Registration failed");
            return View(registerRequest);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authorize");
        }

        public IActionResult ChangePassword()
        {
            try
            {
                var username = User.Identity?.Name;
                if (username is not null)
                {
                    var changePasswordDto = new ChangePasswordDto
                    {
                        Username = username,
                        CurrentPassword = string.Empty,
                        NewPassword = string.Empty,
                        ConfirmPassword = string.Empty
                    };

                    return View(changePasswordDto);
                }

                _toastNotification.AddErrorToastMessage("User not found");
                return View();
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Invalid credentials");
                return View(changePasswordDto);
            }
                
            try
            {
                var result = await _client.PostAsync<Result<IdentityResult>>(ApiConstants.ChangePassword, changePasswordDto);

                if (result.IsError)
                {
                    _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                    return View(changePasswordDto);
                }

                _toastNotification.AddSuccessToastMessage("Password changed successfully");
                return RedirectToAction(nameof(ChangePassword));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Exception: {ex.Message}");
                return View(changePasswordDto);
            }
        }
    }
}
