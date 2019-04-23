using Plugins.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tenants.Services.Tenant1
{
    [Pluggable]
    public class Tenant1Service
    {
        public string SayHello()
        {
            return "Hello world Tenant 1";
        }
    }
}
