using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CentralizedApps.Controllers
{
    [Route("[controller]")]
    public class MunicipalityController : Controller
    {
        private readonly IUnitOfWork _Unit;


        public MunicipalityController(IUnitOfWork Unit)
        {
            _Unit = Unit;
        }

        [HttpPost]
        public async Task<IActionResult> Muni(RegisterMunicipalityDto dto)
        {
            return Ok();
        }

    }
}