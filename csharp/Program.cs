using Ninject;
using SpreadyMcSpreader.Abstractions;
using System.Reflection;

namespace SpreadyMcSpreader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());

                var mcSpreaderCalculator = kernel.Get<IMcSpreaderCalculator>();
                mcSpreaderCalculator.CalculateAndDisplay(args[0]);
            }
        }
    }
}
