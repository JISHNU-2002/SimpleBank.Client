using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;

namespace SimpleBank.Client.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public AccountController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Dashboard()
        {
            var accountNumber = User.FindFirst("AccountNumber")?.Value;

            if (string.IsNullOrEmpty(accountNumber))
                return RedirectToAction("Login", "Authorize");

            try
            {
                var result = await _client.PostAsync<Result<DashboardDto>>($"{ApiConstants.Dashboard}/{accountNumber}", new { });

                if (!result.IsError)
                    return View(result.Response);

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Profile()
        {
            try
            {
                var formId = Convert.ToInt32(User.FindFirst("FormId")?.Value);
                var result = await _client.GetAsync<Result<ProfileDto>>($"{ApiConstants.GetProfile}/{formId}");

                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> UpdateProfile()
        {
            try
            {
                var formId = Convert.ToInt32(User.FindFirst("FormId")?.Value);
                var result = await _client.GetAsync<Result<ProfileDto>>($"{ApiConstants.GetProfile}/{formId}");

                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
                return View(profileDto);
            try
            {
                var result = await _client.PutAsync<Result<ProfileDto>>(ApiConstants.UpdateProfile, profileDto);

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Profile updated successfully");
                    return RedirectToAction(nameof(Profile));
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();

            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<UsersDto>>($"{ApiConstants.GetUserById}/{id}");

                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            try
            {
                var result = await _client.PostAsync<Result<UsersDto>>($"{ApiConstants.DeleteUserById}/{id}", new {});
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("User deleted successfully");
                    return RedirectToAction("Users", "AdminDashboard");
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }
    }
}
