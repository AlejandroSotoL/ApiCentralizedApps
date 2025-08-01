using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IPaymentHistoryService
    {
        

        Task<IEnumerable<PaymentHistoryUserListDto>> getAllPaymentHistoryByIdAsync(int id);
        Task<PaymentHistory> createPaymentHistory(CreatePaymentHistoryDto paymentHistoryDto);
        Task<ValidationResponseDto> UpdatePaymentHistory(int id, int idStatusType);



    }
}