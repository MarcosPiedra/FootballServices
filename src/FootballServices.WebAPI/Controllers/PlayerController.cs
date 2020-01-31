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
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> logger;
        private readonly IPlayerService playerService;
        private readonly IMapper mapper;

        public PlayerController(ILogger<PlayerController> logger,
                                IPlayerService playerService,
                                IMapper mapper)
        {
            this.logger = logger;
            this.playerService = playerService;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PlayerResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PlayerResponse>>> GetAll()
        {
            var playerList = await this.playerService.GetAllAsync();

            if (playerList is null)
            {
                return Ok();
            }

            var playerResponseList = this.mapper.Map<List<Player>, List<PlayerResponse>>(playerList);

            return playerResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PlayerResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PlayerResponse>> Get(int id)
        {
            var player = await this.playerService.FindAsync(id);

            if (player is null)
            {
                return NotFound();
            }

            var playerResponse = this.mapper.Map<Player, PlayerResponse>(player);

            return playerResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePlayer([FromBody] PlayerRequest playerRequest)
        {
            if (playerRequest is null)
            {
                return BadRequest();
            }

            var player = this.mapper.Map<PlayerRequest, Player>(playerRequest);

            await this.playerService.AddAsync(player);
            
            return CreatedAtAction(nameof(CreatePlayer), new { id = player.Id, version = this.HttpContext.GetRequestedApiVersion().ToString() }, player.Id);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Updateplayer(int id, [FromBody] PlayerRequest playerRequest)
        {
            if (id < 1 || playerRequest is null)
            {
                return BadRequest();
            }

            var playerToUpdate = await this.playerService.FindAsync(id);
            if (playerToUpdate is null)
            {
                return NotFound();
            }

            var player = this.mapper.Map(playerRequest, playerToUpdate);

            await this.playerService.UpdateAsync(player);
            
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteplayerByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var playerToDelete = await this.playerService.FindAsync(id);

            if (playerToDelete is null)
            {
                return NotFound();
            }

            await this.playerService.RemoveAsync(playerToDelete);

            return Ok();
        }
    }
}