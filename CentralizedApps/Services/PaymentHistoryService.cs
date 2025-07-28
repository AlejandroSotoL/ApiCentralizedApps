using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentHistoryUserListDto>> getAllPaymentHistoryByIdAsync(int id)
        {

            return await _unitOfWork.paymentHistoryRepository.GetAllPaymentHistoryByIdAsync(paymentHistory => paymentHistory.UserId == id);
        }


        public async Task<PaymentHistory> createPaymentHistory(PaymentHistoryDto paymentHistoryDto)
        {
            PaymentHistory paymentHistory = new PaymentHistory
            {

                UserId = paymentHistoryDto.UserId,
                Amount = paymentHistoryDto.Amount,
                PaymentDate = paymentHistoryDto.PaymentDate,
                Status = paymentHistoryDto.Status,
                MunicipalityId = paymentHistoryDto.MunicipalityId,
                StatusType = paymentHistoryDto.StatusType,
                MunicipalityProceduresId = paymentHistoryDto.MunicipalityProceduresId
            

            };

            await _unitOfWork.paymentHistoryRepository.AddAsync(paymentHistory);
            return paymentHistory;
        }


        public void UpdatePaymentHistory(PaymentHistory paymentHistory, PaymentHistoryDto paymentHistoryDto)
        {
            paymentHistory.Id = paymentHistory.Id;
            paymentHistory.UserId = paymentHistoryDto.UserId;
            paymentHistory.Amount = paymentHistoryDto.Amount;
            paymentHistory.PaymentDate = paymentHistoryDto.PaymentDate;
            paymentHistory.Status = paymentHistoryDto.Status;
            paymentHistory.StatusType = paymentHistoryDto.StatusType;
            paymentHistory.MunicipalityId = paymentHistoryDto.MunicipalityId;
            paymentHistory.MunicipalityProceduresId = paymentHistoryDto.MunicipalityProceduresId;

            _unitOfWork.paymentHistoryRepository.Update(paymentHistory);

        }
    }
}
