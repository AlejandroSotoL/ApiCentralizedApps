using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Services
{
    public class MunicipalityServices : IMunicipalityServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityServices> _logger;
        public MunicipalityServices(ILogger<MunicipalityServices> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AddMunicipalityAsync(CompleteMunicipalityDto dto)
        {
            var strategy = _unitOfWork.GetExecutionStrategy();

            return await strategy.ExecuteAsync<object, bool>(
                null,
                async (context, _, cancellationToken) =>
                {
                    await using var transaction = await _unitOfWork.BeginTransactionAsync();

                    try
                    {
                        // 1. Registrar Departamento
                        Department departament;
                        try
                        {
                            var departamentRepo = _unitOfWork.genericRepository<Department>();
                            departament = await departamentRepo.FindAsync_Predicate(d => d.Name == dto.DepartmentDto);

                            if (departament == null)
                            {
                                departament = new Department { Name = dto.DepartmentDto };
                                departamentRepo.AddAsync(departament);
                                await _unitOfWork.CompleteAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error al registrar el departamento '{dto.DepartmentDto}': {ex.Message}", ex);
                        }

                        // 2. Validar Tema existente
                        Theme tema;
                        try
                        {
                            var temaRepo = _unitOfWork.genericRepository<Theme>();
                            tema = await temaRepo.FindAsync_Predicate(t => t.BackGroundColor == dto.ThemeDto);

                            if (tema == null)
                                throw new Exception("El tema ingresado no existe. Debe crearlo en el endpoint /api/newTheme.");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error al obtener el tema '{dto.ThemeDto}': {ex.Message}", ex);
                        }

                        // 3. Registrar Municipio
                        try
                        {
                            var municipality = new Municipality
                            {
                                Name = dto.NameDto,
                                DepartmentId = departament.Id,
                                ThemeId = tema.Id,
                                Domain = dto.DomainDto,
                                UserFintech = BCrypt.Net.BCrypt.HashPassword(dto.UserFintechDto),
                                PasswordFintech = BCrypt.Net.BCrypt.HashPassword(dto.PasswordFintechDto),
                                IsActive = dto.IsActiveDto
                            };

                            var municipioRepo = _unitOfWork.genericRepository<Municipality>();
                            municipioRepo.AddAsync(municipality);
                            await _unitOfWork.CompleteAsync();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error al registrar el municipio '{dto.NameDto}': {ex.Message}", ex);
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(sqlEx, "SQL Error Code {ErrorCode}: {Message}", sqlEx.Number, sqlEx.Message);
                        if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                            throw new ApplicationException("Ya existe un registro con los datos ingresados.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Error al procesar AddMunicipalityAsync: {Message}", ex.Message);
                        throw new ApplicationException($"Ocurrió un error inesperado al registrar el municipio: {ex.Message}", ex);
                    }
                },
                null,
                CancellationToken.None
            );
        }

        public async Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelations()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de todos los municipios con relaciones.");

                var response = _unitOfWork.genericRepository<Municipality>();
                var entities = await response.GetAllWithNestedIncludesAsync(query =>
                    query
                        .Include(r => r.CourseSportsFacilities)
                            .ThenInclude(r => r.SportFacilities)
                        .Include(r => r.Department)
                        .Include(r => r.MunicipalityProcedures)
                            .ThenInclude(r => r.Procedures)
                        .Include(r => r.MunicipalitySocialMedia)
                            .ThenInclude(r => r.SocialMediaType)
                        .Include(r => r.PaymentHistories)
                        .Include(r => r.Theme)
                );

                if (entities == null || !entities.Any())
                {
                    _logger.LogWarning("No se encontraron municipios con relaciones.");
                    return new List<GetMunicipalitysDto>();
                }

                _logger.LogInformation("Se encontraron {Count} municipios con relaciones.", entities.Count);

                return _mapper.Map<List<GetMunicipalitysDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la información de los Municipios: {Message}", ex.Message);
                throw new ApplicationException($"Ocurrió un error inesperado al obtener los municipios: {ex.Message}", ex);
            }
        }


        public async Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId)
        {
            try
            {
                _logger.LogInformation("Iniciando filtrado de municipios por DepartamentoId = {DepartamentId}", DepartamentId);

                var municipios = await _unitOfWork.genericRepository<Municipality>().GetAllAsync();

                if (municipios == null || !municipios.Any())
                {
                    _logger.LogWarning("No se encontraron municipios en la base de datos.");
                    return new List<JustMunicipalitysDto>();
                }

                var filtrados = municipios
                    .Where(m => m.DepartmentId == DepartamentId)
                    .Select(m => new JustMunicipalitysDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Domain = m.Domain
                    })
                    .ToList();

                if (!filtrados.Any())
                {
                    _logger.LogWarning("No se encontraron municipios con DepartamentoId = {DepartamentId}", DepartamentId);
                }
                else
                {
                    _logger.LogInformation("Se encontraron {Count} municipios con DepartamentoId = {DepartamentId}", filtrados.Count, DepartamentId);
                }

                return filtrados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la información de los Municipios filtrados por DepartamentoId = {DepartamentId}: {Message}", DepartamentId, ex.Message);
                throw new ApplicationException($"Ocurrió un error inesperado al obtener los municipios: {ex.Message}", ex);
            }
        }

    }
}

