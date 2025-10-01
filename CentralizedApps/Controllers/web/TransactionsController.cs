using CentralizedApps.Models.Dtos;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CentralizedApps.Controllers.Web
{
    public class TransactionsController : Controller
    {
        private readonly IPaymentHistoryService _paymentHistoryService;

        public TransactionsController(IPaymentHistoryService paymentHistoryService)
        {
            _paymentHistoryService = paymentHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _paymentHistoryService.getAllPaymentHistory();
            var paymentsByStatus = model
                .GroupBy(p => p.StatusType)
                .Select(g => new { Status = g.Key, Count = g.Count(), Amount = g.Sum(x => x.Amount ?? 0) })
                .ToList();

            var paymentsByMonth = model
                .Where(p => p.PaymentDate.HasValue)
                .GroupBy(p => new { p.PaymentDate.Value.Year, p.PaymentDate.Value.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Month:00}/{g.Key.Year}",
                    Total = g.Sum(x => x.Amount ?? 0)
                })
                .OrderBy(x => x.Month)
                .ToList();

            var paymentsByProcedure = model
                .Where(p => p.MunicipalityProcedure?.Procedure?.Name != null)
                .GroupBy(p => p.MunicipalityProcedure.Procedure.Name)
                .Select(g => new { Procedure = g.Key, Total = g.Sum(x => x.Amount ?? 0) })
                .ToList();

            ViewBag.PaymentsByStatus = paymentsByStatus;
            ViewBag.PaymentsByMonth = paymentsByMonth;
            ViewBag.PaymentsByProcedure = paymentsByProcedure;
            var culture = new CultureInfo("es-CO");
            ViewBag.TotalPayments = model.Sum(p => p.Amount ?? 0).ToString("C0", culture);

            return View(model ?? new List<CompletePaymentDto>());
        }
        [HttpPost]
        public async Task<IActionResult> ReturnByValidatorDni(string? FilterDni, string? FilterDate)
        {
            try
            {
                var information = await _paymentHistoryService.getAllPaymentHistory();
                DateTime? parsedDate = null;
                if (!string.IsNullOrWhiteSpace(FilterDate) &&
                    DateTime.TryParse(FilterDate, out var date))
                {
                    parsedDate = date.Date;
                }

                if (!string.IsNullOrWhiteSpace(FilterDni))
                {
                    information = information
                        .Where(x => x.User?.NationalId == FilterDni)
                        .ToList();
                }

                if (parsedDate.HasValue)
                {
                    information = information
                        .Where(x => x.PaymentDate.HasValue &&
                                    x.PaymentDate.Value.Date == parsedDate.Value)
                        .ToList();
                }
                var culture = new CultureInfo("es-CO");
                ViewBag.TotalPayments = information.Sum(p => p.Amount ?? 0).ToString("C1", culture);
                return View("Index", information);
            }
            catch (Exception)
            {
                return View("Index", new List<CompletePaymentDto>());
            }
        }

    }
}
