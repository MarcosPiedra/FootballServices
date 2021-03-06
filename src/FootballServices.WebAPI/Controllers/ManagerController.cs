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
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> logger;
        private readonly IManagerService managerServices;
        private readonly IValidator<Manager> validator;
        private readonly IMapper mapper;

        public ManagerController(ILogger<ManagerController> logger,
                                 IManagerService managerServices,
                                 IValidator<Manager> validator,
                                 IMapper mapper)
        {
            this.logger = logger;
            this.managerServices = managerServices;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ManagerResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ManagerResponse>>> GetAllManagers()
        {
            var managerList = await this.managerServices.GetAllAsync();

            if (managerList == null)
            {
                return Ok();
            }

            var managerResponseList = this.mapper.Map<List<Manager>, List<ManagerResponse>>(managerList);

            logger.LogInformation($"GetAllManagers");

            return managerResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ManagerResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ManagerResponse>> GetManager(int id)
        {
            var manager = await this.managerServices.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            var managerResponse = this.mapper.Map<Manager, ManagerResponse>(manager);

            logger.LogInformation($"GetManager {id}");

            return managerResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateManager([FromBody] ManagerRequest managerRequest)
        {
            if (managerRequest == null)
            {
                return BadRequest();
            }

            var manager = this.mapper.Map<ManagerRequest, Manager>(managerRequest);

            if (!validator.Validate(manager).IsValid)
            {
                return BadRequest();
            }

            await this.managerServices.AddAsync(manager);

            logger.LogInformation($"CreateManager {manager.Id}");

            return CreatedAtAction(nameof(CreateManager), new { id = manager.Id, version = this.HttpContext.GetRequestedApiVersion().ToString() }, manager.Id);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateManager(int id, [FromBody] ManagerRequest managerRequest)
        {
            if (id < 1 || managerRequest == null)
            {
                return BadRequest();
            }

            var managerToUpdate = await this.managerServices.FindAsync(id);
            if (managerToUpdate == null)
            {
                return NotFound();
            }

            var manager = this.mapper.Map(managerRequest, managerToUpdate);

            if (!validator.Validate(manager).IsValid)
            {
                return BadRequest();
            }

            await this.managerServices.UpdateAsync(manager);

            logger.LogInformation($"UpdateManager {manager.Id}");

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteManagerByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var managerToDelete = await this.managerServices.FindAsync(id);

            if (managerToDelete == null)
            {
                return NotFound();
            }

            await this.managerServices.RemoveAsync(managerToDelete);

            logger.LogInformation($"DeleteManagerByIdAsync {managerToDelete.Id}");

            return Ok();
        }
    }
}