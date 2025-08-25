using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SimpleBank.Client.Models;
using SimpleBank.Client.Repository.Interface;
using SimpleBankClient.Entity.Dto;

namespace SimpleBank.Client.Controllers
{
    [Authorize(Roles ="Manager")]
    public class TransactionsController : Controller
    {
        private readonly IGenericHttpClients _client;
        private readonly IToastNotification _toastNotification;

        public TransactionsController(IGenericHttpClients client,
            IToastNotification toastNotification)
        {
            _client = client;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> TransactionHistory()
        {
            var accountNumber = User.FindFirst("AccountNumber")?.Value;

            if (string.IsNullOrEmpty(accountNumber))
            {
                return RedirectToAction("Login", "Authorize");
            }

            try
            {
                var result = await _client.PostAsync<Result<DashboardDto>>($"{ApiConstants.Dashboard}/{accountNumber}", new { });
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

        public IActionResult Transfer()
        {
            try
            {
                var transferDto = new MoneyTransferDto
                {
                    TransactionType = "Transfer",
                    FromAccountNumber = User.FindFirst("AccountNumber")!.Value
                };

                return View(transferDto);
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(MoneyTransferDto transferDto)
        {
            if (!ModelState.IsValid)
            {
                return View(transferDto);
            }

            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();

            try
            {

                result = await _client.PostAsync<Result<MoneyTransferDto>>(ApiConstants.Transfer, transferDto);

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Transaction successful");
                    return RedirectToAction("Dashboard", "Account");
                }

                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View(transferDto);
            }
            catch(Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View(transferDto);
            }
        }

        public IActionResult Deposit()
        {
            try
            {
                var depositDto = new MoneyTransferDto
                {
                    TransactionType = "Deposit",
                    FromAccountNumber = User.FindFirst("AccountNumber")!.Value
                };

                return View(depositDto);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(MoneyTransferDto depositDto)
        {
            if (!ModelState.IsValid)
            {
                return View(depositDto);
            }

            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();

            try
            {
                result = await _client.PostAsync<Result<MoneyTransferDto>>(ApiConstants.Deposit, depositDto);                         

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Deposit successful");
                    return RedirectToAction("ManagerDashboard", "AdminDashboard");
                }


                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View(depositDto);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        public IActionResult Withdraw()
        {
            try
            {
                var withdrawDto = new MoneyTransferDto
                {
                    TransactionType = "Withdraw",
                    ToAccountNumber = User.FindFirst("AccountNumber")!.Value
                };

                return View(withdrawDto);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(MoneyTransferDto withdrawDto)
        {
            if (!ModelState.IsValid)
            {
                return View(withdrawDto);
            }

            Result<MoneyTransferDto> result = new Result<MoneyTransferDto>();

            try
            {
                result = await _client.PostAsync<Result<MoneyTransferDto>>(ApiConstants.Withdraw, withdrawDto);

                if (!result.IsError)
                {
                    _toastNotification.AddSuccessToastMessage("Withdraw successful");
                    return RedirectToAction("ManagerDashboard", "AdminDashboard");
                }


                _toastNotification.AddErrorToastMessage(result.Errors[0].ErrorMessage);
                return View(withdrawDto);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View();
            }
        }
    }
}
