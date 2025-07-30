using System.Linq.Expressions;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IPaymentHistoryRepository : IGenericRepository<PaymentHistory>
    {
        Task<IEnumerable<PaymentHistoryUserListDto>> GetAllPaymentHistoryByIdAsync(Expression<Func<PaymentHistory, bool>> filtro);
        Task<PaymentHistory> GetPaymentHistoryByIdAsync(Expression<Func<PaymentHistory, bool>> filtro);
    }
}