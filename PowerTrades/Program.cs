using Axpo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PowerTrades;
using PowerTrades.Application.Inbound;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Date;
using PowerTrades.Infrastructure.Outbound;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;


ProgramParameters programParameters = ProgramParametersReader.Read(args);

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

ConfigureLogging(builder, programParameters);

builder.Services.AddSingleton<IPowerService,PowerService>();
builder.Services.AddSingleton<IPowerTradeRepository, AxpoPowerTradeRepository>();
builder.Services.AddSingleton<IPowerTradeForecastReportRepository, CsvFilePowerTradeForecastReportRepository>();
builder.Services.AddSingleton<IDateTimeService, RealDateTimeService>();
builder.Services.AddSingleton<GeneratePowerTradeForecastReportUseCase>();

using IHost host = builder.Build();

int interval = programParameters.ExecutionIntervalInMinutes * 60 * 1000;
//Source: ChatGPT
using var timer = new Timer(async _ => await Run(host.Services, programParameters), null, 0, interval);

Console.WriteLine("PowerTrades is running. Press any key to stop it...");
Console.ReadLine();
Console.WriteLine("Application finished...");

static async Task Run(IServiceProvider hostProvider, ProgramParameters programParameters)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var useCase = provider.GetRequiredService<GeneratePowerTradeForecastReportUseCase>();

    await useCase.GenerateForecastReport(programParameters.DestinationFolder);
    Console.WriteLine("PowerTrades is running. Press any key to stop it...");
}

static void ConfigureLogging(HostApplicationBuilder builder, ProgramParameters programParameters)
{
    //See https://github.com/serilog/serilog-expressions?tab=readme-ov-file#formatting-with-expressiontemplate
    var logFormat = "[{@t:HH:mm:ss}][{@l:u3}][{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}]: {@m}\n{@x}";
    builder.Logging.ClearProviders(); //To remove default console log: https://github.com/serilog/serilog-aspnetcore/issues/92#issuecomment-502317672
    builder.Services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(new ExpressionTemplate(logFormat, theme: TemplateTheme.Code))
            .WriteTo.File(path: Path.Combine(programParameters.DestinationFolder,"logs.txt"), rollingInterval: RollingInterval.Day, formatter: new ExpressionTemplate(logFormat))
            .CreateLogger()));
}