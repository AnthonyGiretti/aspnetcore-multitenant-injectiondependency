using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoMultiTenancy.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Plugins.Abstractions.Providers;

namespace DemoMultiTenancy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IPluginProvider<IHelloWorldService> _helloWorldProvider;

        public ValuesController(IPluginProvider<IHelloWorldService> helloWorldProvider)
        {
            _helloWorldProvider = helloWorldProvider;
        }

        // GET api/values/5
        [HttpGet("{tenant}")]
        public ActionResult<string> Get(string tenant)
        {
            var instance = _helloWorldProvider.GetInstance(tenant);
            return instance.SayHello();
        }
    }
}
