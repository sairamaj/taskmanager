using Autofac.Core;

namespace Utils.Core.Registration
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        T Resolve<T>(params Parameter[] parameters);
    }
}
