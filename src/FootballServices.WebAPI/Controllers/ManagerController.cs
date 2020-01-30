using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballServices.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> logger;

        public ManagerController(ILogger<ManagerController> logger)
        {
            this.logger = logger;
        }

        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Index()
        {
            NotFound();
            BadRequest();
            return View();
        }
    }
}