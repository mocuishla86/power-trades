using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerTrades.Application.inbound;
using PowerTrades.Application.outbound;
using PowerTrades.Domain.date;
using PowerTrades.Domain.Date;
using PowerTrades.Infrastructure.Outbound;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IPowerTradeRepository, FakePowerTradeRepository>();
builder.Services.AddSingleton<IDateTimeService, RealDateTimeService>();
builder.Services.AddSingleton<GeneratePowerTradeForecastReportUseCase>();

using IHost host = builder.Build();
Run(host.Services);

static void Run(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var useCase = provider.GetRequiredService<GeneratePowerTradeForecastReportUseCase>();

    useCase.GenerateForecastReport();

    Console.WriteLine("...");
}
