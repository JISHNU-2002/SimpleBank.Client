using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;

namespace SimpleBank.Client.Controllers
{
    [Authorize(Roles ="Manager")]
    public class AccountTypeController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public AccountTypeController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public IActionResult AddAccountType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAccountType(AccountTypeDto dto)
        {
            try
            {
                var result = await _client.PostAsync<Result<AccountTypeDto>>(ApiConstants.AddAccountType, dto);
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Account type added successfully");
                    return RedirectToAction("AccountTypes", "AdminDashboard");
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

        public async Task<IActionResult> EditAccountType(int id)
        {
            try
            {
                var result = await _client.GetAsync<Result<AccountTypeDto>>($"{ApiConstants.GetAccountTypeById}/{id}");
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
        public async Task<IActionResult> EditAccountType(AccountTypeDto dto)
        {
            try
            {
                var result = await _client.PutAsync<Result<AccountTypeDto>>(ApiConstants.UpdateAccountType, dto);
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Account type updated successfully");
                    return RedirectToAction("AccountTypes", "AdminDashboard");
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

        public async Task<IActionResult> DeleteAccountType(int id)
        {
            try
            {
                var result = await _client.GetAsync<Result<AccountTypeDto>>($"{ApiConstants.GetAccountTypeById}/{id}");
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

        [HttpPost, ActionName("DeleteAccountType")]
        public async Task<IActionResult> DeleteAccountTypeConfirm(int id)
        {
            try
            {
                var result = await _client.DeleteAsync<Result<AccountTypeDto>>($"{ApiConstants.DeleteAccountTypeById}/{id}");
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Account type deleted successfully");
                    return RedirectToAction("AccountTypes", "AdminDashboard");
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
