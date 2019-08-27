using System;
using Autofac;
using Demo1Task.Repository;

namespace Demo1Task
{
    public class TaskInitializer
    {
        public static void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<DemoRepository>().As<IDemoRepository>();
        }
    }
}
