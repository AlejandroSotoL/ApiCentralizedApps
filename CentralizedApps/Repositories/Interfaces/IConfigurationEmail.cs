using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IConfigurationEmail
    {
        Task<ValidationResponseDto> EmailConfiguration(string Subject, string Body, string To);

        Task<ValidationResponseExtraDto> SendEmailValidationCode(string To);
    }
}