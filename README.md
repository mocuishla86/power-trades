# power-trades

TODOS:
- Create the console adapter. We will have to read from console and call use case
- Add logs. 
- Create the Dll adapter. We will have to convert from DllModel to DomainModel and bacwards
- Create the CSV adapter. We will have to convert from Report to CSV and write it to file.
- Consider retries and delays
- Execute several times, separatred by a period. 

# Links

- Logs:
	- https://learn.microsoft.com/en-us/dotnet/core/extensions/logging
	- https://stackoverflow.com/a/75830211
- Dependency Injection: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
- Reading Command Line Arguments: https://www.pietschsoft.com/post/2024/04/23/csharp-console-accept-commandline-arguments
- Retries, etc: https://github.com/App-vNext/Polly