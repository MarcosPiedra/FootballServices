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
    public class MatchController : Controller
    {
        private readonly ILogger<MatchController> logger;
        private readonly IMatchService matchService;
        private readonly IPlayerService playerService;
        private readonly IManagerService managerService;
        private readonly IRefereeService refereeService;
        private readonly IMapper mapper;

        public MatchController(ILogger<MatchController> logger,
                               IMatchService matchService,
                               IPlayerService playerService,
                               IManagerService managerService,
                               IRefereeService refereeService,
                               IMapper mapper)
        {
            this.logger = logger;
            this.matchService = matchService;
            this.playerService = playerService;
            this.managerService = managerService;
            this.refereeService = refereeService;
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

            return matchResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MatchResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MatchResponse>> Get(int id)
        {
            var match = await this.matchService.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            var matchResponse = this.mapper.Map<Match, MatchResponse>(match);

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

            if (await ThereAreNotFoundAsync(matchRequest))
            {
                return NotFound();
            }

            var match = this.mapper.Map<MatchRequest, Match>(matchRequest);

            await this.matchService.AddAsync(match);

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

            if (await ThereAreNotFoundAsync(matchRequest))
            {
                return NotFound();
            }

            var matchToUpdate = await this.matchService.FindAsync(id);
            if (matchToUpdate == null)
            {
                return NotFound();
            }

            var match = this.mapper.Map(matchRequest, matchToUpdate);

            await this.matchService.UpdateAsync(match);

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

            return Ok();
        }

        private async Task<bool> ThereAreNotFoundAsync(MatchRequest matchRequest)
        {
            foreach (var p in matchRequest.AwayTeam)
                if (await playerService.FindAsync(p) == null)
                    return true;

            foreach (var p in matchRequest.HouseTeam)
                if (await playerService.FindAsync(p) == null)
                    return true;

            if (await managerService.FindAsync(matchRequest.AwayManager) == null)
                return true;

            if (await managerService.FindAsync(matchRequest.HouseManager) == null)
                return true;

            if (await refereeService.FindAsync(matchRequest.Referee) == null)
                return true;

            return false;
        }
    }
}