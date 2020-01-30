using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FootballServices.Domain;
using FootballServices.Domain.DTOs;
using FootballServices.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballServices.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> logger;
        private readonly IManagerService managerServices;
        private readonly IMapper mapper;

        public ManagerController(ILogger<ManagerController> logger,
                                 IManagerService managerServices,
                                 IMapper mapper)
        {
            this.logger = logger;
            this.managerServices = managerServices;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ManagerResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ManagerResponse>>> GetAll()
        {
            var managerList = await this.managerServices.GetAllAsync();

            if (managerList is null)
            {
                return Ok();
            }

            var mapped = this.mapper.Map<List<Manager>, List<ManagerResponse>>(managerList);

            return mapped;
        }
    }
}