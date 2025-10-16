using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IPaymentHistoryService
    {


        Task<IEnumerable<PaymentHistoryUserListDto>> getAllPaymentHistoryByIdAsync(int id);
        Task<PaymentHistory> createPaymentHistory(CreatePaymentHistoryDto paymentHistoryDto);
        Task<ValidationResponseDto> UpdatePaymentHistory(int id, int idStatusType);
        Task<ValidationResponseDto> DeletePaymentHistory(int idUser, int idHistory);
        Task<List<AvailibityDto>> getAllAvailibity();
        Task<List<CompletePaymentDto>> getAllPaymentHistory();

    }
}