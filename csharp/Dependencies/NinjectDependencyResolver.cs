using Ninject.Modules;
using SpreadyMcSpreader.Abstractions;
using SpreadyMcSpreader.Concrete;

namespace SpreadyMcSpreader.Dependencies
{
    public class NinjectDependencyResolver : NinjectModule
    {
        public override void Load()
        {
            Bind<IMcSpreaderCalculator>().To<McSpreaderCalculator>();
            Bind<IResultDisplayer>().To<ResultDisplayer>();
        }
    }
}
