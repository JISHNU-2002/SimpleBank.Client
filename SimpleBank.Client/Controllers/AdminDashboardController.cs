using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;
using System.Collections.Generic;

namespace SimpleBank.Client.Controllers
{
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public AdminDashboardController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public IActionResult SuperAdminDashboard()
        {
            return View();
        }

        public async Task<IActionResult> ManagerDashboard()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<ApplicationFormDto>>>(ApiConstants.GetAllForms);

                if (!result.IsError)
                    return View(result.Response);

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Branches()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<BranchDto>>>(ApiConstants.GetAllBranches);

                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Users()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<UsersDto>>>(ApiConstants.GetAllUsersWithDetails);
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> AccountTypes()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<AccountTypeDto>>>(ApiConstants.GetAllAccountTypes);
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Transactions()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<TransactionDetailsDto>>>(ApiConstants.GetAllTransactionsDetails);
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Applications()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<ApplicationFormDto>>>(ApiConstants.GetAllForms);
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Roles()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<IdentityRole>>>(ApiConstants.GetAllRoles);
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
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
