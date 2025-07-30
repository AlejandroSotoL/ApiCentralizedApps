using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IPaymentHistoryService
    {
        

        Task<IEnumerable<PaymentHistoryUserListDto>> getAllPaymentHistoryByIdAsync(int id);
        Task<PaymentHistory> createPaymentHistory(PaymentHistoryDto paymentHistoryDto);
        void UpdatePaymentHistory(PaymentHistory paymentHistory, PaymentHistoryDto paymentHistoryDto);



    }
}