using DemoMultiTenancy.Services.Abstractions;
using Plugins.Abstractions.Attributes;

namespace Tenants.Services.Tenant2
{
    [Pluggable]
    public class Tenant2Service : IHelloWorldService
    {
        public string SayHello()
        {
            return "Hello world Tenant 2";
        }
    }
}
