using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Insurance.Specs.ProductApi
{
    public class ControllerTestFixture: IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                .ConfigureWebHostDefaults(
                    b => b.UseUrls("http://localhost:5002")
                        .UseStartup<ControllerTestStartup>()
                )
                .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }
}