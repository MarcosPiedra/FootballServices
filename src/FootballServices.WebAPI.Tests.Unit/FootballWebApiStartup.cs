using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.WebAPI.Tests.Unit
{
    public class MarketingTestsStartup : Startup
    {
        public MarketingTestsStartup(IWebHostEnvironment env) : base(env)
        {
        }

    }
}
