using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;
using System.Reflection;

namespace SimpleBank.Client.Controllers
{
    public class ApplicationFormController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public ApplicationFormController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        private async Task<List<SelectListItem>> GetAllAccountTypes()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<AccountTypeDto>>>(ApiConstants.GetAllAccountTypes);
                var accountTypesList = new List<SelectListItem>();
                if (result.Response != null)
                {
                    foreach (var accType in result.Response)
                    {
                        accountTypesList.Add(new SelectListItem()
                        {
                            Text = accType.TypeName,
                            Value = accType.TypeId.ToString()
                        });
                    }
                    return accountTypesList;
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return new List<SelectListItem>();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error fetching account types: {ex.Message}");
                return new List<SelectListItem>();
            }
        }
        private async Task<List<SelectListItem>> GetAllBranches()
        {
            try
            {
                var result = await _client.GetAsync<Result<List<BranchDto>>>(ApiConstants.GetAllBranches);
                var branchesList = new List<SelectListItem>();
                if (result.Response != null)
                {
                    foreach (var branch in result.Response)
                    {
                        branchesList.Add(new SelectListItem()
                        {
                            Text = branch.BranchName,
                            Value = branch.IFSC
                        });
                    }
                    return branchesList;
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return new List<SelectListItem>();
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error fetching branches: {ex.Message}");
                return new List<SelectListItem>();
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var accTypes = await GetAllAccountTypes();
                var branches = await GetAllBranches();
                NewApplicationDto newApplication = new()
                {
                    AccountTypes = accTypes,
                    Branches = branches
                };
                return View(newApplication);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error loading application form: {ex.Message}");
                return View(new NewApplicationDto());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewApplicationDto newApplication)
        {
            try
            {
                var result = await _client.PostAsync<Result<NewApplicationDto>>(ApiConstants.ApplicationForm, newApplication);
                
                if (!result.IsError && result.Response != null)
                {
                    _toastNotification.AddSuccessToastMessage("Application submitted successfully");

                    var formId = result.Response.FormId;
                    return RedirectToAction(nameof(Success), new
                    {
                        formId = result.Response.FormId,
                        email = result.Response.Email
                    });
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return View(newApplication);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error submitting application: {ex.Message}");
                return View(newApplication);
            }
        }

        public IActionResult Success(int formId, string email)
        {
            try
            {
                var applicationSubmit = new ApplicationSubmitDto
                {
                    FormId = formId,
                    Email = email
                };
                return View(applicationSubmit);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error loading success page: {ex.Message}");
                return View();
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _client.PostAsync<Result<ApplicationFormDto>>($"{ApiConstants.ApproveForm}/{id}", new { });

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Application approved successfully");
                    return RedirectToAction("ManagerDashboard", "AdminDashboard");
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);
                return RedirectToAction("Applications", "AdminDashboard");
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error approving application: {ex.Message}");
                return RedirectToAction("Applications", "AdminDashboard");
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var result = await _client.PostAsync<Result<ApplicationFormDto>>($"{ApiConstants.RejectForm}/{id}", new { });

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Application rejected successfully");
                    return RedirectToAction("ManagerDashboard", "AdminDashboard");
                }

                _toastNotification.AddWarningToastMessage(result.Errors[0].ErrorMessage);               
                return RedirectToAction("Applications", "AdminDashboard");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error rejecting application: {ex.Message}");
                return RedirectToAction("Applications", "AdminDashboard");
            }
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> FormDetails(int id)
        {
            try
            {
                var result = await _client.GetAsync<Result<ApplicationFormDto>>($"{ApiConstants.FormDetails}/{id}");
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("User details loaded successfully");
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
    }
}
