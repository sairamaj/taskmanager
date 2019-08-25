using Autofac;
using TaskManager.Repository;

namespace TaskManager
{
    class TaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskRepository>().As<ITaskRepository>().SingleInstance();
            base.Load(builder);
        }
    }
}
