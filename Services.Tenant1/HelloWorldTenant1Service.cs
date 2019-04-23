using Plugins.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using DemoMultiTenancy.Services.Abstractions;

namespace Tenants.Services.Tenant1
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
