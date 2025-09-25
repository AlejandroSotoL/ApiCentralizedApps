using System.Net;
using System.Text.Json;
using CentralizedApps.Models.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Middelware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string mensaje;

            if (FindSqlException(ex) is SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Error SQL detectado");

                mensaje = sqlEx.Number switch
                {
                    4060 => "No se puede abrir la base de datos. Verifique si existe o si tiene permisos.",
                    18456 => "Fallo de inicio de sesión. Verifique credenciales.",
                    -1 => "Conexión fallida: No se puede establecer conexión con el servidor SQL.",
                    2 => "Servidor SQL no encontrado o inaccesible.",
                    53 => "No se puede encontrar la instancia del servidor SQL.",
                    _ => $"Error en la base de datos. Código: {sqlEx.Number}"
                };
            }
            else if (ex is DbUpdateException dbUpdateEx)
            {
                _logger.LogError(dbUpdateEx, "Error al actualizar la base de datos.");
                mensaje = "Error al actualizar la base de datos. Contacte al administrador.";
            }
            else if (ex is TaskCanceledException)
            {
                _logger.LogError(ex, "Timeout o tarea cancelada.");
                mensaje = "La operación tardó demasiado tiempo o fue cancelada.";
            }
            else
            {
                _logger.LogError(ex, "Error no controlado.");
                mensaje = "Error interno del servidor. Contacte al administrador.";
            }

            var result = JsonSerializer.Serialize(new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = response.StatusCode,
                SentencesError = mensaje
            });

            try
            {
                await response.WriteAsync(result);
            }
            catch (Exception writeEx)
            {
                _logger.LogError(writeEx, "No se pudo escribir la respuesta de error.");
            }
        }
    }

    private SqlException? FindSqlException(Exception ex)
    {
        while (ex != null)
        {
            if (ex is SqlException sqlEx) return sqlEx;
            ex = ex.InnerException!;
        }
        return null;
    }
}
