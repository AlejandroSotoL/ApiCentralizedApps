using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.EmailDto;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CentralizedApps.Repositories
{
    public class ConfigurationEmail : IConfigurationEmail
    {

        private readonly CentralizedAppsDbContext _context;
        private readonly IUnitOfWork _Unit;
        public ConfigurationEmail(CentralizedAppsDbContext context, IUnitOfWork Unit)
        {
            _context = context;
            _Unit = Unit;
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

                    credential.Timeout = 10000;
                    await credential.SendMailAsync(email);
                }

                return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    SentencesError = "¡Correo enviado!",
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

        public async Task<ValidationResponseExtraDto> SendEmailValidationCode([FromBody] string To)
        {
            try
            {
                var response = await _Unit.genericRepository<User>().FindAsync_Predicate(x => x.Email == To);
                if (response == null)
                {
                    return new ValidationResponseExtraDto
                    {
                        CodeStatus = 404,
                        SentencesError = "Usuario no encontrado",
                        BooleanStatus = false,
                        ExtraData = null
                    };

                }
                var random = new Random();
                int validationCode = random.Next(100000, 999999);
                string codeString = validationCode.ToString();

                string subject = "¿Olvidaste tu contraseña?, aqui está tu código de validación.";
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "indexRecoveryCode.html");

                if (!File.Exists(templatePath))
                {
                 return new ValidationResponseExtraDto
                 {
                  CodeStatus = 500,
                  SentencesError = "Error: No se encontró la plantilla de correo (index.html).",
                  BooleanStatus = false,
                  ExtraData = null
                };
                }

              // 3. Leer la plantilla HTML
        string body = await File.ReadAllTextAsync(templatePath);

        // 4. Reemplazar los marcadores de posición
        body = body.Replace("{{Email}}", To);
        body = body.Replace("{{Code1}}", codeString[0].ToString());
        body = body.Replace("{{Code2}}", codeString[1].ToString());
        body = body.Replace("{{Code3}}", codeString[2].ToString());
        body = body.Replace("{{Code4}}", codeString[3].ToString());
        body = body.Replace("{{Code5}}", codeString[4].ToString());
        body = body.Replace("{{Code6}}", codeString[5].ToString());

        // --- FIN DE MODIFICACIÓN ---

        var result = await EmailConfiguration(subject, body, To);

                return new ValidationResponseExtraDto
                {
                    CodeStatus = result.CodeStatus,
                    SentencesError = result.SentencesError,
                    BooleanStatus = result.BooleanStatus,
                    ExtraData = result.BooleanStatus ? validationCode.ToString() : null
                };
            }
            catch (Exception e)
            {
                return new ValidationResponseExtraDto
                {
                    CodeStatus = 500,
                    SentencesError = "Error sending validation code: " + e.Message,
                    BooleanStatus = false,
                    ExtraData = null
                };
            }
        }

        public async Task<ValidationResponseDto> SendEmailPanic(EmailDtoPanic emailDto)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "panicTemplate.html");

                string alertTimestamp = DateTime.Now.ToString("hh:mm tt");

                string body = await File.ReadAllTextAsync(templatePath);

                body = body.Replace("{{userName}}", emailDto.Name);
                body = body.Replace("{{userEmail}}", emailDto.UserEmail);

                body = body.Replace("{{alertTimestamp}}", alertTimestamp);

                body = body.Replace("{{userPhone}}", emailDto.Phone);
                body = body.Replace("{{locationCoordinates}}", emailDto.locationCoordinates);

                return await EmailConfiguration(emailDto.Subject, body, emailDto.To);
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    SentencesError = "Error procesando correo de pánico: " + e.Message,
                    BooleanStatus = false
                };
            }
        }

        public async Task<ValidationResponseDto> SendEmailReservation(EmailDtoReservations emailDto)
        {
            try
            {
                string fileName = "";

                switch (emailDto.Id)
                {
                    case "1":
                        fileName = "reservationCourseTemplate.html"; 
                        break;
                    case "2":
                        fileName = "reservationSportTemplate.html"; 
                        break;
                    default:
                        return new ValidationResponseDto
                        {
                            CodeStatus = 400,
                            SentencesError = "Error: Tipo de reservación (Id) no válido.",
                            BooleanStatus = false
                        };
                }
        
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", fileName);

   
                string alertTimestamp = DateTime.Now.ToString("hh:mm tt");
                string body = await File.ReadAllTextAsync(templatePath);

          
                body = body.Replace("{{guestName}}", emailDto.NameUser);
                body = body.Replace("{{guestEmail}}", emailDto.EmailUser);
                body = body.Replace("{{guestPhone}}", emailDto.PhoneUser);
                body = body.Replace("{{receptionTimestamp}}", alertTimestamp);

                string dateText = emailDto.DateReservation.HasValue
                     ? emailDto.DateReservation.Value.ToString("dd/MM/yyyy HH:mm")
                     : "Fecha por confirmar";

                body = body.Replace("{{DateReservation}}", dateText);

                body = body.Replace("{{Object}}", emailDto.Type);
                body = body.Replace("{{guestIdUser}}", emailDto.IdUser);
                body = body.Replace("{{reservationType}}", emailDto.Type);
                body = body.Replace("{{headerTitle}}", emailDto.Type);


                return await EmailConfiguration(emailDto.Subject, body, emailDto.To);
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    CodeStatus = 500,
                    SentencesError = "Error procesando correo de reservación: " + e.Message,
                    BooleanStatus = false
                };
            }
        }


    }
}