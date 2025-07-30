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
            await _unitOfWork.SaveChangesAsync();
            return paymentHistory;
        }


        public async Task<ValidationResponseDto> UpdatePaymentHistory(int id, int idStatusType)
        {

            var paymentHistory = await _unitOfWork.paymentHistoryRepository.GetPaymentHistoryByIdAsync(paymentHistory => paymentHistory.Id == id);
            if (paymentHistory == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "notfund"
                };
            }

            paymentHistory.StatusType = idStatusType;

            _unitOfWork.paymentHistoryRepository.Update(paymentHistory);

            await _unitOfWork.SaveChangesAsync();
            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };

        }
    }
}
