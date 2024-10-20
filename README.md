# power-trades

## How to launch the application

- .NET 8.0 required. 
- Compile the application. From the root folder, execute

```
dotnet build
```

- Run it:

```
.\PowerTrades\bin\debug\net8.0\PowerTrades.exe  --executionIntervalInMinutes=1 --destinationFolder=.\PowerTrades\bin\debug\net8.0
````

- CSV Files and logs will appear inside folder `.\PowerTrades\bin\debug\net8.0`.

# Links

- Logs:
	- https://learn.microsoft.com/en-us/dotnet/core/extensions/logging
	- https://stackoverflow.com/a/75830211
- Dependency Injection: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
- Reading Command Line Arguments: https://www.pietschsoft.com/post/2024/04/23/csharp-console-accept-commandline-arguments