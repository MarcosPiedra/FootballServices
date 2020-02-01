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
    public class RefereeController : Controller
    {
        private readonly ILogger<RefereeController> logger;
        private readonly IRefereeService refereeService;
        private readonly IMapper mapper;

        public RefereeController(ILogger<RefereeController> logger,
                                 IRefereeService refereeService,
                                 IMapper mapper)
        {
            this.logger = logger;
            this.refereeService = refereeService;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RefereeResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<RefereeResponse>>> GetAll()
        {
            var refereeList = await this.refereeService.GetAllAsync();

            if (refereeList == null)
            {
                return Ok();
            }

            var refereeResponseList = this.mapper.Map<List<Referee>, List<RefereeResponse>>(refereeList);

            return refereeResponseList;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RefereeResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RefereeResponse>> Get(int id)
        {
            var referee = await this.refereeService.FindAsync(id);

            if (referee == null)
            {
                return NotFound();
            }

            var refereeResponse = this.mapper.Map<Referee, RefereeResponse>(referee);

            return refereeResponse;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateReferee([FromBody] RefereeRequest refereeRequest)
        {
            if (refereeRequest == null)
            {
                return BadRequest();
            }

            var referee = this.mapper.Map<RefereeRequest, Referee>(refereeRequest);

            await this.refereeService.AddAsync(referee);
            
            return CreatedAtAction(nameof(CreateReferee), new { id = referee.Id, version = this.HttpContext.GetRequestedApiVersion().ToString() }, referee.Id);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateReferee(int id, [FromBody] RefereeRequest refereeRequest)
        {
            if (id < 1 || refereeRequest == null)
            {
                return BadRequest();
            }

            var refereeToUpdate = await this.refereeService.FindAsync(id);
            if (refereeToUpdate == null)
            {
                return NotFound();
            }

            var referee = this.mapper.Map(refereeRequest, refereeToUpdate);

            await this.refereeService.UpdateAsync(referee);
            
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteRefereeByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var refereeToDelete = await this.refereeService.FindAsync(id);

            if (refereeToDelete == null)
            {
                return NotFound();
            }

            await this.refereeService.RemoveAsync(refereeToDelete);

            return Ok();
        }
    }
}