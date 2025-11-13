using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.EmailDto;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IConfigurationEmail
    {
        Task<ValidationResponseDto> EmailConfiguration(string Subject, string Body, string To);
        Task<ValidationResponseExtraDto> SendEmailValidationCode(string To);
        Task<ValidationResponseDto> SendEmailPanic(EmailDtoPanic emailDto);
        Task<ValidationResponseDto> SendEmailReservation(EmailDtoReservations emailDto);
    }
}