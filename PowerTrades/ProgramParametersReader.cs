using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades
{
    //Copied from https://www.pietschsoft.com/post/2024/04/23/csharp-console-accept-commandline-arguments
    public class ProgramParametersReader
    {
        public static ProgramParameters Read(string[] args)
        {
            try
            {
                Console.WriteLine($"Application started with args: [{String.Join(',', args)}]");
                var arguments = ParseArguments(args);
                if (!arguments.TryGetValue("--executionIntervalInMinutes", out string executionIntervalInMinutes))
                {
                    throw new ArgumentException("--executionIntervalInMinutes parameter not found");
                }

                if (!arguments.TryGetValue("--destinationFolder", out string destinationFolder))
                {
                    throw new ArgumentException("--destinationFolder parameter not found");
                }

                return new ProgramParameters
                {
                    ExecutionIntervalInMinutes = int.Parse(executionIntervalInMinutes),
                    DestinationFolder = destinationFolder
                };

            }
            catch(Exception e)
            { 
                Console.WriteLine($"Error reading parameters {e}");
                PrintHelp();
                throw;
            }
        }

        static Dictionary<string, string> ParseArguments(string[] args)
        {
            var arguments = new Dictionary<string, string>();

            foreach (var arg in args)
            {
                // Split the argument by '=' to handle key/value pairs
                string[] parts = arg.Split('=');

                // Check if the argument is in the format "key=value"
                if (parts.Length == 2)
                {
                    arguments[parts[0]] = parts[1];
                }
                // If not, assume it's just a named argument without a value
                else
                {
                    arguments[arg] = null;
                }
            }

            return arguments;
        }

        static void PrintHelp()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("------");
            Console.WriteLine("Usage: .\\PowerTrades [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --executionIntervalInMinutes=X        The internval between executions in minutes");
            Console.WriteLine("  --destinationFolder=<folder>          Specify output folder for the forecasdt report");
        }
    }
}
