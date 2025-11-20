using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.EmailDto;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IConfigurationEmail
    {
        Task<ValidationResponseDto> SendEmail(string To, string Subject, string Body);
        Task<ValidationResponseExtraDto> SendEmailValidationCode(string To);
        Task<ValidationResponseDto> SendEmailPanic(EmailDtoPanic emailDto);
        Task<ValidationResponseDto> SendEmailReservation(EmailDtoReservations emailDto);
    }
}