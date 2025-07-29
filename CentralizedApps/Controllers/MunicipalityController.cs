using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories;
using CentralizedApps.Services;
using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CentralizedApps.Controllers
{
    [Route("[controller]")]
    public class MunicipalityController : ControllerBase
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly ILogger<MunicipalityServices> _logger;
        public MunicipalityController(ILogger<MunicipalityServices> logger, IMunicipalityServices MunicipalityServices)
        {
            _MunicipalityServices = MunicipalityServices;
            _logger = logger;
        }

        [HttpPost("/CompleteRegiser_Add")]
        public async Task<IActionResult> AddMunicipalityAsync(CompleteMunicipalityDto dto)
        {
            try
            {
                var result = await _MunicipalityServices.AddMunicipalityAsync(dto);
                return Ok(new { success = result, message = "Municipio registrado correctamente." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en AddMunicipality");
                return StatusCode(500, new
                {
                    error = "Ocurrió un error interno del servidor."
                });
            }
        }

        [HttpGet("/GetAllMunicipalityWithRelations")]
        public async Task<IActionResult> GetAllMunicipalityWithRelations()
        {
            try
            {
                var response = await _MunicipalityServices.GetAllMunicipalityWithRelations();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllMunicipalityWithRelations: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }


        [HttpGet("/GetJustMunicipalitysByIdDepartaments_{DepartamentId}")]
        public async Task<IActionResult> JustMunicipalitys(int DepartamentId)
        {
            try
            {
                var relations = await _MunicipalityServices.justMunicipalitysDtos(DepartamentId);
                if (relations == null || !relations.Any())
                {
                    _logger.LogWarning("No se encontraron municipios para el DepartamentoId: {DepartamentId}", DepartamentId);
                    return NotFound("No se encontraron municipios.");
                }

                _logger.LogInformation("Municipios encontrados: {Count}", relations.Count);
                return Ok(relations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en JustMunicipalitys: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }

        [HttpGet("/GetJustGetMunicipalityWithRelations_{IdMunicipality}")]
        public async Task<IActionResult> GetJust(int IdMunicipality)
        {
            var response = await _MunicipalityServices.JustGetMunicipalityWithRelations(IdMunicipality);

            if (response == null)
                return NotFound($"No se encontró el municipio con ID {IdMunicipality}");
            return Ok(response);
        }
    }
}