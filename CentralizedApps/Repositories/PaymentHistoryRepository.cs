
using System.Linq.Expressions;
using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    
    public class PaymentHistoryRepository : GenericRepository<PaymentHistory>, IPaymentHistoryRepository
    {

        private readonly CentralizedAppsDbContext _context;
    private readonly DbSet<PaymentHistory> _DbSet;
        public PaymentHistoryRepository(CentralizedAppsDbContext Context) : base(Context)
        {
            _context = Context;
            _DbSet = _context.Set<PaymentHistory>();
        }

        public async Task<IEnumerable<PaymentHistoryUserListDto>> GetAllPaymentHistoryByIdAsync(Expression<Func<PaymentHistory, bool>> filtro)
        {
            var listpaymentHistoryUser = await _context.PaymentHistories
                                    .Where(filtro)
                                    .Include(pa => pa.User)
                                    .Include(pa => pa.Municipality)
                                    .Include(pa => pa.StatusTypeNavigation)
                                    .Include(pa => pa.MunicipalityProcedures)
                                        .ThenInclude(mp => mp.Procedures)
                                    .Select(pa => new PaymentHistoryUserListDto
                                    {
                                        Id = pa.Id,
                                        Amount = pa.Amount,
                                        Status = pa.Status,
                                        PaymentDate = pa.PaymentDate,
                                        UserFirtName = pa.User!.FirstName,
                                        MunicipalityName = pa.Municipality!.Name,
                                        StatusType = pa.StatusTypeNavigation!.TypeStatus,
                                        ProcedureName = pa.MunicipalityProcedures!.Procedures!.Name
                                    })
                                    .ToListAsync();
            return listpaymentHistoryUser;

        }

        public async Task<PaymentHistory> GetPaymentHistoryByIdAsync(Expression<Func<PaymentHistory, bool>> filtro)
        {
            var paymentHistoryUser = await _context.PaymentHistories
            .Include(p => p.User)
            .Include(p => p.Municipality)
            .Include(p => p.StatusTypeNavigation)
            .Include(p => p.MunicipalityProcedures)
                .ThenInclude(mp => mp.Procedures)
            .FirstOrDefaultAsync(filtro);

            return paymentHistoryUser?? new PaymentHistory();

        }
    }
}