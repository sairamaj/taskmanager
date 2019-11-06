using Autofac;
using TaskManager.Repository;

namespace TaskManager
{
    /// <summary>
    /// Task module.
    /// </summary>
    internal class TaskModule : Module
    {
        /// <summary>
        /// Autofac override.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="ContainerBuilder"/> instance.
        /// </param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskRepository>().As<ITaskRepository>().SingleInstance();
            base.Load(builder);
        }
    }
}
