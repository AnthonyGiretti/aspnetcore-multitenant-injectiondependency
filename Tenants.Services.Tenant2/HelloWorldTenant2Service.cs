using DemoMultiTenancy.Services.Abstractions;
using Plugins.Abstractions.Attributes;

namespace Plugins.Services.Tenant2
{
    [Pluggable]
    public class HelloWorldTenant2Service : IHelloWorldService
    {
        public string SayHello()
        {
            return "Hello world Tenant 2";
        }
    }
}
