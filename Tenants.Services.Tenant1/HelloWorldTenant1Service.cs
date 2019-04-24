using Plugins.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using DemoMultiTenancy.Services.Abstractions;

namespace Plugins.Services.Tenant1
{
    [Pluggable]
    public class HelloWorldTenant1Service : IHelloWorldService
    {
        public string SayHello()
        {
            return "Hello world Tenant 1";
        }
    }
}
