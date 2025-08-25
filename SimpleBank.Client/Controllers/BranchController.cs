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
    public class BranchController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public BranchController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public IActionResult AddBranch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(AddBranchDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _client.PostAsync<Result<AddBranchDto>>(ApiConstants.AddBranch, dto);
                    if (!result.IsError)
                    {
                        _toastNotification.AddSuccessToastMessage("Branch added successfully");
                        return RedirectToAction("Branches", "AdminDashboard");
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
            return View();
        }

        public async Task<IActionResult> UpdateBranch(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<UpdateBranchDto>>($"{ApiConstants.GetBranchByIFSC}/{id}");
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
        public async Task<IActionResult> UpdateBranch(UpdateBranchDto dto)
        {
            try
            {
                var result = await _client.PutAsync<Result<UpdateBranchDto>>(ApiConstants.UpdateBranch, dto);
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Branch updated successfully");
                    return RedirectToAction("Branches", "AdminDashboard");
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

        public async Task<IActionResult> DeleteBranch(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<UpdateBranchDto>>($"{ApiConstants.GetBranchByIFSC}/{id}");
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

        [HttpPost, ActionName("DeleteBranch")]
        public async Task<IActionResult> DeleteBranchConfirmed(string id)
        {
            try
            {
                var result = await _client.DeleteAsync<Result<UpdateBranchDto>>($"{ApiConstants.DeleteBranch}/{id}");
                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Branch deleted successfully");
                    return RedirectToAction("Branches", "AdminDashboard");
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

        public async Task<IActionResult> IFSCDetails(string id) 
        {
            try
            {
                var result = await _client.GetAsync<Result<List<IFSCDetailsDto>>>($"{ApiConstants.IFSCDetails}/{id}");
                if (!result.IsError)
                {
                    if(result.Response!.Count == 0)
                    {
                        _toastNotification.AddInfoToastMessage("No users found for this branch");
                        return RedirectToAction("Branches", "AdminDashboard");
                    }

                    _toastNotification.AddSuccessToastMessage("Branch details loaded successfully");
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
