using Autofac;

namespace Utils.Core.Registration
{
    /// <summary>
    /// Core module.
    /// </summary>
    public class CoreModule : Module
    {
        /// <summary>							  
        /// Loading function for Autofac module.
        /// </summary>
        /// <param name="builder">Builder instance <see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandTreeItemViewMapper>().As<ICommandTreeItemViewMapper>().SingleInstance();
        }
    }
}
