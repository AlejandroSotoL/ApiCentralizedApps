
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Entities;
using CentralizedApps.Services.Interfaces;
using AutoMapper;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CentralizedApps.Services
{
    public class ProcedureServices : IProcedureServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityServices> _logger;
        public ProcedureServices(ILogger<MunicipalityServices> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Procedure> createProcedures(CreateProcedureDto procedureDto)
        {
            Procedure procedure = new Procedure
            {
                Name = procedureDto.Name
            };
            await _unitOfWork.genericRepository<Procedure>().AddAsync(procedure);
            await _unitOfWork.SaveChangesAsync();
            return procedure;
        }

        public async Task<DocumentType> createDocumentType(DocumentTypeDto documentTypeDto)
        {
            DocumentType documentType = new DocumentType
            {
                NameDocument = documentTypeDto.NameDocument
            };
            await _unitOfWork.genericRepository<DocumentType>().AddAsync(documentType);
            await _unitOfWork.SaveChangesAsync();
            return documentType;
        }
        public async Task<QueryField> createQueryField(QueryFieldDto queryFieldDto)
        {
            QueryField queryField = new QueryField
            {
                MunicipalityId = queryFieldDto.MunicipalityId,
                FieldName = queryFieldDto.FieldName,
                QueryFieldType = queryFieldDto.QueryFieldType

            };
            await _unitOfWork.genericRepository<QueryField>().AddAsync(queryField);
            await _unitOfWork.SaveChangesAsync();
            return queryField;
        }

        public async Task<Availibity> createAvailibity(CreateAvailibityDto availibityDto)
        {
            Availibity availibity = new Availibity
            {
                TypeStatus = availibityDto.TypeStatus
            };
            await _unitOfWork.genericRepository<Availibity>().AddAsync(availibity);
            await _unitOfWork.SaveChangesAsync();
            return availibity;
        }

        public async Task<bool> AddSocialMediaType(SocialMediaTypeDto socialMediaType)
        {
            try
            {
                if (socialMediaType == null || string.IsNullOrWhiteSpace(socialMediaType.Name))
                {
                    return false;
                }

                var format = new SocialMediaType
                {
                    Name = socialMediaType.Name,
                };
                var repository = _unitOfWork.genericRepository<SocialMediaType>();
                await repository.AddAsync(format);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar SocialMediaType: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("DTO recibido es null.");
                    return false;
                }

                if (dto.MunicipalityId <= 0 || dto.SocialMediaTypeId <= 0)
                {
                    _logger.LogWarning("MunicipalityId o SocialMediaTypeId no válidos.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(dto.Url))
                {
                    _logger.LogWarning("URL de red social está vacía.");
                    return false;
                }

                var entity = _mapper.Map<MunicipalitySocialMedium>(dto);
                var repository = _unitOfWork.genericRepository<MunicipalitySocialMedium>();
                await repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Se agregó correctamente la red social al municipio. ID Municipio: {MunicipalityId}, Tipo: {SocialMediaTypeId}", dto.MunicipalityId, dto.SocialMediaTypeId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar red social al municipio.");
                return false;
            }
        }

        public async Task<ValidationResponseDto> createNewTheme(ThemeDto createThemeDto)
        {
            try
            {
                var theme = _mapper.Map<Theme>(createThemeDto);
                await _unitOfWork.genericRepository<Theme>().AddAsync(theme);
                var affectedRows = await _unitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 201,
                        SentencesError = $"Tema creado correctamente: {createThemeDto.NameTheme}, filas afectadas: {affectedRows}."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = $"Error al crear el tema: {createThemeDto.NameTheme}. No se guardaron cambios."
                    };
                }
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al crear el tema: {e.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> UpdateTheme(int id, ThemeDto procedureDto)
        {
            try
            {
                if (procedureDto == null || string.IsNullOrWhiteSpace(procedureDto.BackGroundColor))
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = $"It's required: {id}"
                    };
                }

                var theme = await _unitOfWork.genericRepository<Theme>()
                    .FindAsync_Predicate(x => x.Id == id);
                if (theme == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = $"Esta id no existe: {id}"
                    };
                }

                theme.NameTheme = procedureDto.NameTheme;
                theme.BackGroundColor = procedureDto.BackGroundColor;
                theme.PrimaryColor = procedureDto.PrimaryColor;
                theme.SecondaryColor = procedureDto.SecondaryColor;
                theme.SecondaryColorBlack = procedureDto.SecondaryColorBlack;
                theme.OnPrimaryColorLight = procedureDto.OnPrimaryColorLight;
                theme.OnPrimaryColorDark = procedureDto.OnPrimaryColorDark;

                _unitOfWork.genericRepository<Theme>().Update(theme);
                await _unitOfWork.SaveChangesAsync();
                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Editado"
                };

            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error al actualizar el tema: " + ex
                };
            }
        }


        public async Task<Course> createCourse(CreateCourseDto createCourseDto)
        {
            Course course = new Course
            {
                MunicipalityId = createCourseDto.MunicipalityId,
                Name = createCourseDto.Name,
                Post = createCourseDto.Post,
                Get = createCourseDto.Get,
                IsActive = createCourseDto.IsActive
            };
            await _unitOfWork.genericRepository<Course>().AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
            return course;
        }


        public async Task<SportsFacility> createSportsFacility(CreateSportsFacilityDto createSportsFacilityDto)
        {
            SportsFacility sportsFacility = new SportsFacility
            {
                MunicipalityId = createSportsFacilityDto.MunicipalityId,
                Name = createSportsFacilityDto.Name,
                Get = createSportsFacilityDto.Get,
                CalendaryPost = createSportsFacilityDto.CalendaryPost,
                ReservationPost = createSportsFacilityDto.ReservationPost,
                IsActive = createSportsFacilityDto.IsActive
            };
            await _unitOfWork.genericRepository<SportsFacility>().AddAsync(sportsFacility);
            await _unitOfWork.SaveChangesAsync();
            return sportsFacility;
        }


        public async Task<SocialMediaType> createSocialMediaType(CreateSocialMediaTypeDto createSocialMediaTypeDto)
        {
            SocialMediaType socialMediaType = new SocialMediaType
            {
                Name = createSocialMediaTypeDto.Name,

            };
            await _unitOfWork.genericRepository<SocialMediaType>().AddAsync(socialMediaType);
            await _unitOfWork.SaveChangesAsync();
            return socialMediaType;
        }

        public async Task<ValidationResponseDto> updateDocumentType(int id, DocumentTypeDto updatedocumentTypeDto)
        {
            var documentType = await _unitOfWork.genericRepository<DocumentType>().GetByIdAsync(id);
            if (documentType == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            documentType.NameDocument = updatedocumentTypeDto.NameDocument;
            _unitOfWork.genericRepository<DocumentType>().Update(documentType);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }

        public async Task<ValidationResponseDto> updateQueryField(int id, QueryFieldDto updatequeryFieldDto)
        {
            var queryField = await _unitOfWork.genericRepository<QueryFieldDto>().GetByIdAsync(id);
            if (queryField == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            queryField.MunicipalityId = queryField.MunicipalityId;
            queryField.FieldName = updatequeryFieldDto.FieldName;
            queryField.QueryFieldType = updatequeryFieldDto.QueryFieldType;
            _unitOfWork.genericRepository<QueryFieldDto>().Update(queryField);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> updateAvailibity(int id, CreateAvailibityDto updateAvailibityDto)
        {
            var availibity = await _unitOfWork.genericRepository<Availibity>().GetByIdAsync(id);
            if (availibity == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            availibity.TypeStatus = updateAvailibityDto.TypeStatus;
            _unitOfWork.genericRepository<Availibity>().Update(availibity);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> updateCourse(int id, CreateCourseDto updateCourseDto)
        {
            var Course = await _unitOfWork.genericRepository<Course>().GetByIdAsync(id);
            if (Course == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            Course.MunicipalityId = updateCourseDto.MunicipalityId;
            Course.Name = updateCourseDto.Name;
            Course.Post = updateCourseDto.Post;
            Course.Get = updateCourseDto.Get;
            _unitOfWork.genericRepository<Course>().Update(Course);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> updateSportsFacility(int id, CreateSportsFacilityDto updateSportsFacilityDto)
        {
            var SportsFacility = await _unitOfWork.genericRepository<SportsFacility>().GetByIdAsync(id);
            if (SportsFacility == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            SportsFacility.MunicipalityId = updateSportsFacilityDto.MunicipalityId;
            SportsFacility.Name = updateSportsFacilityDto.Name;
            SportsFacility.Get = updateSportsFacilityDto.Get;
            SportsFacility.ReservationPost = updateSportsFacilityDto.ReservationPost;
            SportsFacility.CalendaryPost = updateSportsFacilityDto.CalendaryPost;
            SportsFacility.IsActive = updateSportsFacilityDto.IsActive;
            _unitOfWork.genericRepository<SportsFacility>().Update(SportsFacility);
            await _unitOfWork.SaveChangesAsync();


            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> updateSocialMediaType(int id, CreateSocialMediaTypeDto updateSocialMediaTypeDto)
        {
            var socialMediaType = await _unitOfWork.genericRepository<SocialMediaType>().GetByIdAsync(id);
            if (socialMediaType == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound "
                };
            }

            socialMediaType.Name = updateSocialMediaTypeDto.Name;

            _unitOfWork.genericRepository<SocialMediaType>().Update(socialMediaType);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> updateMunicipalitySocialMedium(int id, CreateMunicipalitySocialMediumDto updateMunicipalitySocialMediumDto)
        {
            var municipalitySocialMedium = await _unitOfWork.genericRepository<MunicipalitySocialMedium>().GetByIdAsync(id);
            if (municipalitySocialMedium == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound "
                };
            }

            municipalitySocialMedium.MunicipalityId = updateMunicipalitySocialMediumDto.MunicipalityId;
            municipalitySocialMedium.SocialMediaTypeId = updateMunicipalitySocialMediumDto.SocialMediaTypeId;
            municipalitySocialMedium.Url = updateMunicipalitySocialMediumDto.Url;
            municipalitySocialMedium.IsActive = updateMunicipalitySocialMediumDto.IsActive;

            _unitOfWork.genericRepository<MunicipalitySocialMedium>().Update(municipalitySocialMedium);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }


        public async Task<ValidationResponseDto> AsingProccessToMunicipality(MunicipalityProcedureAddDto addMunicipalityProcedures)
        {
            try
            {
                if (addMunicipalityProcedures == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Los datos del procedimiento son requeridos."
                    };
                }

                var map = _mapper.Map<MunicipalityProcedure>(addMunicipalityProcedures);
                var call = _unitOfWork.genericRepository<MunicipalityProcedure>();
                await call.AddAsync(map);
                await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Proceso asignado correctamente al municipio."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"{ex.Message} => Error al asignar procesos al municipio."
                };
            }
        }

        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            try
            {
                var response = await _unitOfWork.genericRepository<DocumentType>().GetAllAsync();
                return response.ToList();
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException("Error al obtener los tipos de documentos: " + ex.Message);
            }
        }

        public async Task<ValidationResponseDto> UpdateMunicipality(int id, CompleteMunicipalityDto municipalityDto)
        {
            try
            {
                if (municipalityDto == null || string.IsNullOrWhiteSpace(municipalityDto.Name))
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Los datos del municipio son requeridos."
                    };
                }

                var existingMunicipality = await _unitOfWork
                    .genericRepository<Municipality>()
                    .GetByIdAsync(id);

                if (existingMunicipality == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "El municipio no existe."
                    };
                }

                // ======= Validar si Theme existe =======
                Theme themeEntity = null;
                if (!string.IsNullOrWhiteSpace(municipalityDto.Theme))
                {
                    themeEntity = await _unitOfWork
                        .genericRepository<Theme>()
                        .FindAsync_Predicate(x => x.NameTheme == municipalityDto.Theme);

                    if (themeEntity == null)
                    {
                        return new ValidationResponseDto
                        {
                            BooleanStatus = false,
                            CodeStatus = 400,
                            SentencesError = $"El tema '{municipalityDto.Theme}' no existe. Debe crearlo antes de poder asignarlo al municipio."
                        };
                    }
                }

                // ======= Buscar o crear Department =======
                Department departmentEntity = null;
                if (!string.IsNullOrWhiteSpace(municipalityDto.Department))
                {
                    departmentEntity = await _unitOfWork
                        .genericRepository<Department>()
                        .FindAsync_Predicate(x => x.Name == municipalityDto.Department);

                    if (departmentEntity == null)
                    {
                        departmentEntity = new Department { Name = municipalityDto.Department };
                        await _unitOfWork.genericRepository<Department>().AddAsync(departmentEntity);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                var response = await _unitOfWork.genericRepository<ShieldMunicipality>()
                .FindAsync_Predicate(x => x.NameOfMunicipality == municipalityDto.NameEscudo);

                if (response == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Debe de ir a crear el shield en Create/ShieldMuniciaplity"
                    };
                }


                existingMunicipality.Name = municipalityDto.Name;
                existingMunicipality.IdShield = response.Id;
                existingMunicipality.DataPrivacy = municipalityDto.DataPrivacy;
                existingMunicipality.DataProcessingPrivacy = municipalityDto.DataProcessingPrivacy;
                existingMunicipality.EntityCode = municipalityDto.EntityCode;
                existingMunicipality.IsActive = municipalityDto.IsActive;
                existingMunicipality.Domain = municipalityDto.Domain;
                existingMunicipality.UserFintech = municipalityDto.UserFintech;
                existingMunicipality.PasswordFintech = municipalityDto.PasswordFintech;

                if (departmentEntity != null)
                    existingMunicipality.Department = departmentEntity;

                if (themeEntity != null)
                    existingMunicipality.Theme = themeEntity;

                _unitOfWork.genericRepository<Municipality>().Update(existingMunicipality);
                await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Municipio actualizado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error al actualizar el municipio: " + ex.Message
                };
            }
        }

        public async Task<ValidationResponseDto> createNewTypeProcedure(CreateProcedureDto createProcedureDto)
        {
            try
            {
                if (createProcedureDto == null || string.IsNullOrWhiteSpace(createProcedureDto.Name))
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Los datos del procedimiento son requeridos."
                    };
                }

                var procedure = new Procedure
                {
                    Name = createProcedureDto.Name
                };
                await _unitOfWork.genericRepository<Procedure>().AddAsync(procedure);
                var response = await _unitOfWork.SaveChangesAsync();
                if (response <= 0)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al crear el procedimiento."
                    };
                }

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Procedimiento creado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al crear el procedimiento: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> createNewNotice(NewsByMunicipalityDto newsByMunicipalityDto)
        {
            try
            {
                var exixsted = await _unitOfWork.genericRepository<Municipality>()
                    .FindAsync_Predicate(x => x.Id == newsByMunicipalityDto.IdMunicipality);

                if (exixsted == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Municipio no encontrado."
                    };
                }

                var news = _mapper.Map<NewsByMunicipality>(newsByMunicipalityDto);
                await _unitOfWork.genericRepository<NewsByMunicipality>().AddAsync(news);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (newsByMunicipalityDto != null && rows > 0)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 201,
                        SentencesError = "Noticia creada correctamente. lineas afectadas: " + rows
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al crear la noticia."
                    };

                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al crear la noticia: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> updateNews(int id, NewsByMunicipalityDto newsByMunicipalityDto)
        {
            try
            {
                if (newsByMunicipalityDto == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Datos inválidos."
                    };
                }

                var existidNew = await _unitOfWork.genericRepository<NewsByMunicipality>()
                    .FindAsync_Predicate(x => x.Id == id);

                if (existidNew == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Noticia no encontrada."
                    };
                }

                var search = await _unitOfWork.genericRepository<Municipality>()
                    .FindAsync_Predicate(x => x.Id == newsByMunicipalityDto.IdMunicipality);
                if (search == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "Municipio no encontrado."
                    };
                }
                _mapper.Map(newsByMunicipalityDto, existidNew);
                _unitOfWork.genericRepository<NewsByMunicipality>().Update(existidNew);

                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = "Noticia actualizada correctamente. lineas afectadas: " + rows
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al actualizar la noticia."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar la noticia: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> UpdateProcedureStatus(int Id, bool status)
        {
            try
            {
                var find = await _unitOfWork.genericRepository<MunicipalityProcedure>()
                    .FindAsync_Predicate(x => x.Id == Id);
                if (find == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontró el procedimiento."
                    };
                }

                find.IsActive = status;
                _unitOfWork.genericRepository<MunicipalityProcedure>().Update(find);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0 && find != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = "Estado del procedimiento actualizado correctamente. " + rows + " filas afectadas."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al actualizar el estado del procedimiento."
                    };

                }
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = "Error al actualizar el estado del procedimiento."
                };
            }
        }

        public async Task<ValidationResponseDto> createShield(ShieldMunicipalityDto createShieldDto)
        {
            try
            {
                if (createShieldDto == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Los datos del escudo son requeridos."
                    };
                }

                var existingShield = await _unitOfWork.genericRepository<ShieldMunicipality>()
                    .FindAsync_Predicate(x => x.NameOfMunicipality == createShieldDto.NameOfMunicipality);

                if (existingShield != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = "Ya existe un escudo con el mismo nombre de municipio."
                    };
                }

                var shield = new ShieldMunicipality
                {
                    NameOfMunicipality = createShieldDto.NameOfMunicipality,
                    Url = createShieldDto.Url
                };

                await _unitOfWork.genericRepository<ShieldMunicipality>().AddAsync(shield);
                await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 201,
                    SentencesError = "Escudo creado correctamente."
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error {ex.Message} al crear el escudo."
                };
            }
        }

        public async Task<ValidationResponseDto> UpdateStatusCourse(int Id, bool status)
        {
            try
            {
                var response = await _unitOfWork.genericRepository<Course>()
                    .FindAsync_Predicate(x => x.Id == Id);
                if (response == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontró el curso."
                    };
                }
                response.IsActive = status;
                _unitOfWork.genericRepository<Course>().Update(response);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0 && response != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = "Estado del curso actualizado correctamente. " + rows + " filas afectadas."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al actualizar el estado del curso."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar el estado del curso: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> UpdateStatusSportFacilietes(int id, bool status)
        {
            try
            {
                var response = await _unitOfWork.genericRepository<SportsFacility>()
                    .FindAsync_Predicate(x => x.Id == id);
                if (response == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontró el Sport."
                    };
                }
                response.IsActive = status;
                _unitOfWork.genericRepository<SportsFacility>().Update(response);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0 && response != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = "Estado del Spot actualizado correctamente. " + rows + " filas afectadas."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al actualizar el estado del Sport."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar el estado del Sport: {ex.Message}"
                };
            }
        }

        public async Task<ValidationResponseDto> UpdateStatusMunicipality(int id, bool status)
        {
            try
            {
                var response = await _unitOfWork.genericRepository<Municipality>()
                    .FindAsync_Predicate(x => x.Id == id);
                if (response == null)
                {
                    return new ValidationResponseDto
                    {
                        SentencesError = "El Municipio no existe",
                        BooleanStatus = false
                    };
                }
                response.IsActive = status;
                _unitOfWork.genericRepository<Municipality>().Update(response);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0 && response != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = $"Estado del Municipio ${response.Name} esta Actualizado correctamente. " + rows + " filas afectadas."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = $"Error al actualizar el Municipio ${response?.Name}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar el estado del Sport: {ex.Message}"
                };
            }
        }
    }
}



