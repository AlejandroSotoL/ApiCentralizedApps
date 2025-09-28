using AutoMapper;
using CentralizedApps.Models.Dtos;
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

        public async Task<ValidationResponseDto> AddMunicipalityAsync(CompleteMunicipalityDto dto)
        {
            var strategy = _unitOfWork.GetExecutionStrategy();
            return await strategy.ExecuteAsync<object, ValidationResponseDto>(
                null,
                async (context, _, cancellationToken) =>
                {
                    await using var transaction = await _unitOfWork.BeginTransactionAsync();

                    try
                    {
                        // ===============================
                        // 1. Registrar Departamento
                        // ===============================
                        Department departament;
                        try
                        {
                            var departamentRepo = _unitOfWork.genericRepository<Department>();
                            departament = await departamentRepo.FindAsync_Predicate(d => d.Name == dto.Department);

                            if (departament == null)
                            {
                                departament = new Department { Name = dto.Department };
                                await departamentRepo.AddAsync(departament);
                                await _unitOfWork.CompleteAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 400,
                                SentencesError = "Error al registrar el departamento: " + ex.Message
                            };
                        }

                        // ===============================
                        // 2. Validar Tema
                        // ===============================
                        Theme tema;
                        try
                        {
                            var temaRepo = _unitOfWork.genericRepository<Theme>();
                            tema = await temaRepo.FindAsync_Predicate(t => t.NameTheme == dto.Theme);

                            if (tema == null)
                            {
                                await transaction.RollbackAsync();
                                return new ValidationResponseDto
                                {
                                    BooleanStatus = false,
                                    CodeStatus = 404,
                                    SentencesError = $"El tema '{dto.Theme}' no existe. Debe crearlo en post/api/newTheme."
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 400,
                                SentencesError = $"Error al obtener el tema '{dto.Theme}': {ex.Message}"
                            };
                        }

                        // ===============================
                        // 3. Validar Escudo
                        // ===============================
                        ShieldMunicipality shield;
                        try
                        {
                            shield = await _unitOfWork.genericRepository<ShieldMunicipality>()
                                        .FindAsync_Predicate(x => x.NameOfMunicipality == dto.NameEscudo);

                            if (shield == null)
                            {
                                await transaction.RollbackAsync();
                                return new ValidationResponseDto
                                {
                                    BooleanStatus = false,
                                    CodeStatus = 404,
                                    SentencesError = $"El escudo '{dto.NameEscudo}' no existe. Debe crearlo en post/api/newShield."
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 400,
                                SentencesError = "Error al asignar el escudo: " + ex.Message
                            };
                        }

                        // ===============================
                        // 4. Validar Banco
                        // ===============================
                        Bank bank;
                        try
                        {
                            var bankRepo = _unitOfWork.genericRepository<Bank>();
                            bank = await bankRepo.FindAsync_Predicate(b => b.NameBank == dto.NameBank);

                            if (bank == null)
                            {
                                await transaction.RollbackAsync();
                                return new ValidationResponseDto
                                {
                                    BooleanStatus = false,
                                    CodeStatus = 404,
                                    SentencesError = $"El banco '{dto.NameBank}' no existe. Debe crearlo en post/api/newBank."
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 400,
                                SentencesError = "Error al asignar el banco: " + ex.Message
                            };
                        }

                        // ===============================
                        // 5. Registrar Municipio
                        // ===============================
                        try
                        {
                            var municipality = new Municipality
                            {
                                Name = dto.Name,
                                EntityCode = dto.EntityCode,
                                DepartmentId = departament.Id,
                                ThemeId = tema.Id,
                                Domain = dto.Domain,
                                UserFintech = dto.UserFintech,
                                PasswordFintech = dto.PasswordFintech,
                                IsActive = dto.IsActive,
                                IdBank = bank.Id,
                                IdShield = shield.Id,
                                DataPrivacy = dto.DataPrivacy,
                                DataProcessingPrivacy = dto.DataProcessingPrivacy
                            };

                            var municipioRepo = _unitOfWork.genericRepository<Municipality>();
                            await municipioRepo.AddAsync(municipality);
                            await _unitOfWork.CompleteAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 400,
                                SentencesError = "Error al registrar el municipio: " + ex.Message
                            };
                        }

                        await transaction.CommitAsync();
                        return new ValidationResponseDto
                        {
                            BooleanStatus = true,
                            CodeStatus = 200,
                            SentencesError = "Municipio registrado exitosamente."
                        };
                    }
                    catch (SqlException sqlEx)
                    {
                        await transaction.RollbackAsync();
                        if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                        {
                            return new ValidationResponseDto
                            {
                                BooleanStatus = false,
                                CodeStatus = 409,
                                SentencesError = "Ya existe un registro con los datos ingresados."
                            };
                        }
                        return new ValidationResponseDto
                        {
                            BooleanStatus = false,
                            CodeStatus = 500,
                            SentencesError = "Error de base de datos: " + sqlEx.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new ValidationResponseDto
                        {
                            BooleanStatus = false,
                            CodeStatus = 500,
                            SentencesError = $"Ocurrió un error inesperado: {ex.Message}"
                        };
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
                var response = _unitOfWork.genericRepository<Municipality>();
                var entities = await response.GetAllWithNestedIncludesAsync(query =>
                    query
                        .Include(r => r.Courses)!
                        .Include(r => r.IdShieldNavigation)!
                        .Include(r => r.IdBankNavigation)!
                        .Include(r => r.NewsByMunicipalities)!
                        .Include(r => r.QueryFields)!
                        .Include(r => r.SportsFacilities)!
                        .Include(r => r.Department)!
                        .Include(r => r.MunicipalityProcedures)!
                            .ThenInclude(r => r.Procedures)!
                        .Include(r => r.MunicipalitySocialMedia)!
                            .ThenInclude(r => r.SocialMediaType)!
                        .Include(r => r.Theme)
                );

                if (entities == null || !entities.Any())
                {
                    return null;
                }
                return _mapper.Map<List<GetMunicipalitysDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ocurrió un error inesperado al obtener los municipios: {ex.Message}", ex);
            }
        }


        public async Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelationsWeb(string? filter)
        {
            try
            {
                var response = _unitOfWork.genericRepository<Municipality>();
                var entities = await response.GetAllWithNestedIncludesAsync(query =>
                    query
                        .Include(r => r.Courses)
                        .Include(r => r.IdShieldNavigation)
                        .Include(r => r.IdBankNavigation)
                        .Include(r => r.NewsByMunicipalities)
                        .Include(r => r.QueryFields)
                        .Include(r => r.SportsFacilities)
                        .Include(r => r.Department)
                        .Include(r => r.MunicipalityProcedures)
                            .ThenInclude(r => r.Procedures)
                        .Include(r => r.MunicipalitySocialMedia)
                            .ThenInclude(r => r.SocialMediaType)
                        .Include(r => r.Theme)
                );

                if (entities == null || !entities.Any())
                {
                    return new List<GetMunicipalitysDto>();
                }

                filter = filter?.ToLower();

                if (!string.IsNullOrEmpty(filter))
                {
                    entities = entities.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(filter)) ||
                        (m.Department != null && m.Department.Name.ToLower().Contains(filter)) ||
                        (m.IdBankNavigation != null && m.IdBankNavigation.NameBank.ToLower().Contains(filter))
                    ).ToList();
                }

                return _mapper.Map<List<GetMunicipalitysDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ocurrió un error inesperado al obtener los municipios: {ex.Message}", ex);
            }
        }

        public async Task<GetMunicipalitysDto?> JustGetMunicipalityWithRelations(int municipalityId)
        {
            try
            {
                var response = _unitOfWork.genericRepository<Municipality>();
                var entity = await response.GetOneWithNestedIncludesAsync(
                    query => query
                        .Include(m => m.Courses)!
                        .Include(r => r.NewsByMunicipalities)!
                        .Include(r => r.IdShieldNavigation)!
                        .Include(r => r.IdBankNavigation)!
                        .Include(r => r.QueryFields)!
                        .Include(m => m.SportsFacilities)!
                        .Include(r => r.Department)
                        .Include(r => r.MunicipalityProcedures)
                            .ThenInclude(r => r.Procedures)
                        .Include(r => r.MunicipalitySocialMedia)
                            .ThenInclude(r => r.SocialMediaType)
                        .Include(r => r.Theme),
                    m => m.Id == municipalityId
                );

                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<GetMunicipalitysDto>(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener municipio con identificador {municipalityId}: {ex.Message}", ex);
            }
        }
        public async Task<MunicipalityDto?> JustGetMunicipalityWithRelationsWeb(int municipalityId)
        {
            try
            {
                var response = _unitOfWork.genericRepository<Municipality>();
                var entity = await response.GetOneWithNestedIncludesAsync(
                    query => query
                        .Include(m => m.Courses)!
                        .Include(r => r.NewsByMunicipalities)!
                        .Include(r => r.IdShieldNavigation)!
                        .Include(r => r.IdBankNavigation)!
                        .Include(r => r.QueryFields)!
                        .Include(m => m.SportsFacilities)!
                        .Include(r => r.Department)
                        .Include(r => r.MunicipalityProcedures)
                            .ThenInclude(r => r.Procedures)
                        .Include(r => r.MunicipalitySocialMedia)
                            .ThenInclude(r => r.SocialMediaType)
                        .Include(r => r.Theme),
                    m => m.Id == municipalityId
                );

                if (entity == null)
                {
                    return null;
                }

                var municipalityDto = _mapper.Map<MunicipalityDto>(entity);
                municipalityDto.Banks = await _unitOfWork.genericRepository<Bank>().GetAllAsync();
                municipalityDto.Departments = await _unitOfWork.genericRepository<Department>().GetAllAsync();
                municipalityDto.Themes = await _unitOfWork.genericRepository<Theme>().GetAllAsync();
                municipalityDto.MunicipalityProcedures = await _unitOfWork.genericRepository<MunicipalityProcedure>().GetAllAsync();
                municipalityDto.MunicipalitySocialMedia = await _unitOfWork.genericRepository<MunicipalitySocialMedium>().GetAllAsync();
                municipalityDto.QueryFields = await _unitOfWork.genericRepository<QueryField>().GetAllAsync();
                return municipalityDto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener municipio con identificador {municipalityId}: {ex.Message}", ex);
            }
        }


        public async Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId)
        {
            try
            {
                var municipios = await _unitOfWork.genericRepository<Municipality>().GetAllAsync();
                if (municipios == null || !municipios.Any())
                {
                    return new List<JustMunicipalitysDto>();
                }

                var filtrados = municipios
                    .Where(m => m.DepartmentId == DepartamentId)
                    .Select(m => new JustMunicipalitysDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Domain = m.Domain,
                        IsActive = m.IsActive
                    })
                    .ToList();

                if (!filtrados.Any())
                {
                    return new List<JustMunicipalitysDto>();
                }
                return filtrados;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ocurrió un error inesperado al obtener los municipios: {ex.Message}", ex);
            }
        }

    }
}

