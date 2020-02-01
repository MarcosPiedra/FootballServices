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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class StatisticsController : Controller
    {
        private readonly ILogger<RefereeController> logger;
        private readonly IStatisticsService statisticsService;
        private readonly IMapper mapper;

        public StatisticsController(ILogger<RefereeController> logger,
                                    IStatisticsService refereeService,
                                    IMapper mapper)
        {
            this.logger = logger;
            this.statisticsService = refereeService;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RefereeResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CardResponse>>> GetYellowCards()
        {
            var stList = await this.statisticsService.GetAsync(StatisticsType.YellowCardsByTeam);

            if (stList == null)
            {
                return Ok();
            }

            var stReponseList = this.mapper.Map<List<Statistic>, List<CardResponse>>(stList);

            return stReponseList;
        }


    }
}