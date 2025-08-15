using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    public class ConfigurationEmail : IConfigurationEmail
    {

        private readonly CentralizedAppsDbContext _context;
        public ConfigurationEmail(CentralizedAppsDbContext context)
        {
            _context = context;
        }

        public async Task<ValidationResponseDto> EmailConfiguration(string Subject, string Body, string To)
        {
            try
            {

                var ExtractConfigurationEmail = await _context.ConfiguracionEmails
                    .Where(x => x.Recurso == "Servicio_Correo")
                    .ToListAsync();

                var createDict = ExtractConfigurationEmail.ToDictionary(y => y.Propiedad.ToLower()
                    , y => y.Valor);

                var RemitentEmail = createDict["correo"];
                var password = createDict["clave"];
                var alias = createDict["alias"];
                var host = createDict["host"];
                var port = int.Parse(createDict["puerto"]);

                var credentials = new NetworkCredential(RemitentEmail, password);
                using (var email = new MailMessage())
                using (var credential = new SmtpClient(host, port))
                {
                    email.From = new MailAddress(RemitentEmail, alias);
                    email.Subject = Convert.ToString(Subject);
                    email.Body = Convert.ToString(Body);
                    email.IsBodyHtml = true;
                    email.To.Add(new MailAddress(Convert.ToString(To)));

                    credential.Credentials = credentials;
                    credential.DeliveryMethod = SmtpDeliveryMethod.Network;
                    credential.UseDefaultCredentials = false;
                    credential.EnableSsl = true;

                    await credential.SendMailAsync(email);
                }

                return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    SentencesError = "Email sent successfully.",
                    BooleanStatus = true
                };
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    SentencesError = "An error occurred while processing your request." + e.Message,
                    BooleanStatus = false
                };
            }
        }
    }
}