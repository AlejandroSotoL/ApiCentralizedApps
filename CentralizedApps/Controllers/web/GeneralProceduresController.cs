using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Controllers.web
{
    public class GeneralProceduresController : Controller
    {
        private readonly IMunicipalityServices _MunicipalityServices;
        private readonly IGeneralProcedures _GeneralProcedure;
        private readonly IProcedureServices _ProcedureServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GeneralProceduresController(IMunicipalityServices MunicipalityServices, IProcedureServices ProcedureServices, IUnitOfWork unitOfWork, IMapper mapper, IGeneralProcedures GeneralProcedure)
        {
            _MunicipalityServices = MunicipalityServices;
            _ProcedureServices = ProcedureServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _GeneralProcedure = GeneralProcedure;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var structs = new CreateMunicipalityProcedures_Web
                {
                    Municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                    Procedures = await _unitOfWork.genericRepository<Procedure>().GetAllAsync(),
                };
                return View(structs);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> BringResultById(int id)
        {
            var response = await _GeneralProcedure.MunicipalityProcedures(id);

            var model = new CreateMunicipalityProcedures_Web
            {
                Municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                Procedures = await _unitOfWork.genericRepository<Procedure>().GetAllAsync(),
                ProceduresWithRelations = response ?? new List<MunicipalityProcedureDto_Reminders>()
            };

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSmothingIt()
        {
            return View();
        }
    }
}
