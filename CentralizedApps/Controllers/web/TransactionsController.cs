using CentralizedApps.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class TransactionsController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<List<PaymentHistoryUserListDto>> Details()
        {

            return new List<PaymentHistoryUserListDto>();

        }
    }
}
