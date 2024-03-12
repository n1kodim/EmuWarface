using BenchmarkDotNet.Running;

namespace EmuWarface.Server.Tests.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}