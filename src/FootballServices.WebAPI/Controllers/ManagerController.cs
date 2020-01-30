﻿using System;
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

            var managerResponseList = this.mapper.Map<List<Manager>, List<ManagerResponse>>(managerList);

            return managerResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ManagerResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ManagerResponse>> Get(int id)
        {
            var manager = await this.managerServices.FindAsync(id);

            if (manager is null)
            {
                return NotFound();
            }

            var managerResponse = this.mapper.Map<Manager, ManagerResponse>(manager);

            return managerResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateManagerAsync([FromBody] ManagerRequest managerRequest)
        {
            if (managerRequest is null)
            {
                return BadRequest();
            }

            var manager = this.mapper.Map<ManagerRequest, Manager>(managerRequest);

            await this.managerServices.AddAsync(manager);

            return CreatedAtAction(nameof(CreateManagerAsync), new { id = manager.Id }, null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateManagerAsync(int id, [FromBody] ManagerRequest managerRequest)
        {
            if (id < 1 || managerRequest is null)
            {
                return BadRequest();
            }

            var managerToUpdate = await this.managerServices.FindAsync(id);
            if (managerToUpdate is null)
            {
                return NotFound();
            }

            var manager = this.mapper.Map(managerRequest, managerToUpdate);

            await this.managerServices.UpdateAsync(manager);

            return CreatedAtAction(nameof(UpdateManagerAsync), new { id = manager.Id }, null);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteManagerByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var managerToDelete = await this.managerServices.FindAsync(id);

            if (managerToDelete is null)
            {
                return NotFound();
            }

            await this.managerServices.RemoveAsync(managerToDelete);

            return NoContent();
        }

    }
}