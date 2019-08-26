
using Autofac;

namespace DemoStartup
{
    public class TaskInitializer
    {
        public static void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>();
        }
    }
}
