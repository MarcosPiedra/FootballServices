﻿using System;
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
    public class PlayerController : Controller
    {
        private readonly ILogger<PlayerController> logger;
        private readonly IValidator<Player> validator;
        private readonly IPlayerService playerService;
        private readonly IMapper mapper;

        public PlayerController(ILogger<PlayerController> logger,
                                IValidator<Player> validator,
                                IPlayerService playerService,
                                IMapper mapper)
        {
            this.logger = logger;
            this.validator = validator;
            this.playerService = playerService;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PlayerResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PlayerResponse>>> GetAllPlayers()
        {
            var playerList = await this.playerService.GetAllAsync();

            if (playerList == null)
            {
                return Ok();
            }

            var playerResponseList = this.mapper.Map<List<Player>, List<PlayerResponse>>(playerList);

            logger.LogInformation($"GetAllPlayers");

            return playerResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PlayerResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PlayerResponse>> GetPlayer(int id)
        {
            var player = await this.playerService.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            var playerResponse = this.mapper.Map<Player, PlayerResponse>(player);

            logger.LogInformation($"GetPlayer {id}");

            return playerResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePlayer([FromBody] PlayerRequest playerRequest)
        {
            if (playerRequest == null)
            {
                return BadRequest();
            }

            var player = this.mapper.Map<PlayerRequest, Player>(playerRequest);

            if (!validator.Validate(player).IsValid)
            {
                return BadRequest();
            }

            await this.playerService.AddAsync(player);

            logger.LogInformation($"CreatePlayer {player.Id}");

            return CreatedAtAction(nameof(CreatePlayer), new { id = player.Id, version = this.HttpContext.GetRequestedApiVersion().ToString() }, player.Id);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdatePlayer(int id, [FromBody] PlayerRequest playerRequest)
        {
            if (id < 1 || playerRequest == null)
            {
                return BadRequest();
            }

            var playerToUpdate = await this.playerService.FindAsync(id);
            if (playerToUpdate == null)
            {
                return NotFound();
            }

            var player = this.mapper.Map(playerRequest, playerToUpdate);

            if (!validator.Validate(player).IsValid)
            {
                return BadRequest();
            }

            await this.playerService.UpdateAsync(player);

            logger.LogInformation($"UpdatePlayer {player.Id}");

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeletePlayerByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var playerToDelete = await this.playerService.FindAsync(id);

            if (playerToDelete == null)
            {
                return NotFound();
            }

            await this.playerService.RemoveAsync(playerToDelete);

            logger.LogInformation($"DeletePlayerByIdAsync {playerToDelete.Id}");

            return Ok();
        }
    }
}