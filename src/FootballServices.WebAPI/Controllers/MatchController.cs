using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FootballServices.Domain;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballServices.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class MatchController : Controller
    {
        private readonly ILogger<MatchController> logger;
        private readonly IMatchService matchService;
        private readonly IPlayerService playerService;
        private readonly IManagerService managerService;
        private readonly IRefereeService refereeService;
        private readonly IValidator<Match> validator;
        private readonly IMapper mapper;

        public MatchController(ILogger<MatchController> logger,
                               IMatchService matchService,
                               IPlayerService playerService,
                               IManagerService managerService,
                               IRefereeService refereeService,
                               IValidator<Match> validator,
                               IMapper mapper)
        {
            this.logger = logger;
            this.matchService = matchService;
            this.playerService = playerService;
            this.managerService = managerService;
            this.refereeService = refereeService;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MatchResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<MatchResponse>>> GetAll()
        {
            var matchList = await this.matchService.GetAllAsync();

            if (matchList == null)
            {
                return Ok();
            }

            var matchResponseList = this.mapper.Map<List<Match>, List<MatchResponse>>(matchList);

            logger.LogInformation($"GetAllMatchesAsync");

            return matchResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MatchResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MatchResponse>> GetMatch(int id)
        {
            var match = await this.matchService.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            var matchResponse = this.mapper.Map<Match, MatchResponse>(match);

            logger.LogInformation($"GetMatch");

            return matchResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateMatch([FromBody] MatchRequest matchRequest)
        {
            if (matchRequest == null)
            {
                return BadRequest();
            }

            var match = this.mapper.Map<MatchRequest, Match>(matchRequest);

            if (!validator.Validate(match).IsValid)
            {
                return BadRequest();
            }

            await this.matchService.AddAsync(match);

            logger.LogInformation($"CreateMatch {match.Id}");

            return CreatedAtAction(nameof(CreateMatch), new { id = match.Id, version = this.HttpContext.GetRequestedApiVersion().ToString() }, match.Id);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateMatch(int id, [FromBody] MatchRequest matchRequest)
        {
            if (id < 1 || matchRequest == null)
            {
                return BadRequest();
            }

            var matchToUpdate = await this.matchService.FindAsync(id);
            if (matchToUpdate == null)
            {
                return NotFound();
            }

            var match = this.mapper.Map(matchRequest, matchToUpdate);

            if (!validator.Validate(match).IsValid)
            {
                return BadRequest();
            }

            await this.matchService.UpdateAsync(match);

            logger.LogInformation($"UpdateMatch {match.Id}");

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteMatchByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var matchToDelete = await this.matchService.FindAsync(id);

            if (matchToDelete == null)
            {
                return NotFound();
            }

            await this.matchService.RemoveAsync(matchToDelete);

            logger.LogInformation($"DeleteMatchByIdAsync {matchToDelete.Id}");

            return Ok();
        }

    }
}