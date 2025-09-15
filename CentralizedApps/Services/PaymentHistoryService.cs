using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentHistoryUserListDto>> getAllPaymentHistoryByIdAsync(int id)
        {
            return await _unitOfWork.paymentHistoryRepository.GetAllPaymentHistoryByIdAsync(paymentHistory => paymentHistory.UserId == id);
        }


        public async Task<PaymentHistory> createPaymentHistory(CreatePaymentHistoryDto paymentHistoryDto)
        {
            PaymentHistory paymentHistory = new PaymentHistory
            {

                UserId = paymentHistoryDto.UserId,
                Amount = paymentHistoryDto.Amount,
                PaymentDate = paymentHistoryDto.PaymentDate,
                Status = paymentHistoryDto.Status,
                StatusType = paymentHistoryDto.StatusType,
                MunicipalityProceduresId = paymentHistoryDto.MunicipalityProceduresId,
                Idimpuesto = paymentHistoryDto.Idimpuesto,
                Factura = paymentHistoryDto.Factura,
                CodigoEntidad = paymentHistoryDto.CodigoEntidad

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

        public async Task<ValidationResponseDto> DeletePaymentHistory(int idUser, int idHistory)
        {
            try
            {
                var paymentHistory = await _unitOfWork.genericRepository<PaymentHistory>().FindAsync_Predicate(ph => ph.UserId == idUser && ph.Id == idHistory);
                if (paymentHistory == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Historial de pago no encontrado"
                    };
                }

                _unitOfWork.paymentHistoryRepository.Delete(paymentHistory);
                var rows = await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "Historial de pago eliminado correctamente - lineas afectadas: " + rows
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                };
            }
        }

        public async Task<List<AvailibityDto>> getAllAvailibity()
        {
            try
            {
                var availibityList = await _unitOfWork.genericRepository<Availibity>().GetAllAsync();
                return _mapper.Map<List<AvailibityDto>>(availibityList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
