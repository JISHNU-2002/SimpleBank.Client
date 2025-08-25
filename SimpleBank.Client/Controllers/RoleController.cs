using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;
using System.Net.Http;

namespace SimpleBank.Client.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public RoleController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Incomplete data");
                return View(roleDto);
            }

            try
            {
                var result = await _client.PostAsync<Result<RoleDto>>(ApiConstants.AddRole, roleDto);
                
                if(result.Response is not null)
                {
                    _toastNotification.AddSuccessToastMessage("Role added successfully");
                    return RedirectToAction("Roles", "AdminDashboard");
                }

                _toastNotification.AddErrorToastMessage("Role not added");
                return View(roleDto);
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<IdentityRole>>($"{ApiConstants.GetRoleById}/{id}");
                if (!result.IsError && result.Response != null)
                {
                    RemoveRoleDto removeRoleDto = new()
                    {
                        RoleId = result.Response.Id,
                        RoleName = result.Response.Name!
                    };

                    return View(removeRoleDto);
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

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            try
            {
                var result = await _client.DeleteAsync<Result<RemoveRoleDto>>($"{ApiConstants.RemoveRole}/{id}");

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Role deleted successfully");
                    return RedirectToAction("Roles", "AdminDashboard");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                    return RedirectToAction("Roles", "AdminDashboard");
                }
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> AddRemoveRoles(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<AddRemoveRoleDto>>($"{ApiConstants.UserRoles}/{id}");

                if(!result.IsError)
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

        [HttpPost]
        public async Task<IActionResult> AddRemoveRoles(AddRemoveRoleDto addRemoveRoleDto)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddWarningToastMessage("Incomplete data");
                return View(addRemoveRoleDto);
            }

            try
            {
                var result = await _client.PostAsync<Result<AddRemoveRole>>(ApiConstants.UpdateUserRoles, addRemoveRoleDto);

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Role assigned successfully");
                    return RedirectToAction("Users", "AdminDashboard");
                }

                ModelState.AddModelError(string.Empty, "Failed to update user roles.");
                _toastNotification.AddWarningToastMessage("Role assigning failed");
                return View(addRemoveRoleDto);
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> RoleDetails(string id)
        {
            try
            {
                var result = await _client.GetAsync<Result<List<UserRolesDto>>>($"{ApiConstants.GetRoleDetailsById}/{id}");
                if (!result.IsError)
                {
                    return View(result.Response);
                }

                _toastNotification.AddWarningToastMessage("No details found for this role");
                return View();
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }
    }
}
