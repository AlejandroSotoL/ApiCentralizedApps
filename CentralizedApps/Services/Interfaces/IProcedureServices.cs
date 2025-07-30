using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IProcedureServices
    {
        Task<bool> createCurseSports(CourseSportsFacilityDto courseSportsFacilityDto);
        
        Task<Procedure> createProcedures(ProcedureDto procedureDto);
        Task<DocumentType> createDocumentType(DocumentTypeDto documentTypeDto);
        Task<QueryField> createQueryField(QueryFieldDto queryFieldDto);
        Task<Availibity> createAvailibity(AvailibityDto availibityDto);
    }
}