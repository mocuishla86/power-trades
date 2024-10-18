using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PowerTrades.Application.Inbound;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Date;
using PowerTrades.Infrastructure.Outbound;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;


Console.WriteLine("Application started");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

ConfigureLogging(builder);

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

    Console.WriteLine("Application finished...");
}

static void ConfigureLogging(HostApplicationBuilder builder)
{
    //See https://github.com/serilog/serilog-expressions?tab=readme-ov-file#formatting-with-expressiontemplate
    var logFormat = "[{@t:HH:mm:ss}][{@l:u3}][{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}]: {@m}\n{@x}";
    builder.Logging.ClearProviders(); //To remove default console log: https://github.com/serilog/serilog-aspnetcore/issues/92#issuecomment-502317672
    builder.Services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(new ExpressionTemplate(logFormat, theme: TemplateTheme.Code))
            .WriteTo.File(path: "logs.txt", rollingInterval: RollingInterval.Day, formatter: new ExpressionTemplate(logFormat))
            .CreateLogger()));
}