using Autofac;
using Utils.Core.Diagnostics;
using Utils.Core.ViewModels;

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
        /// <param name="builder">Builder instance <see cref="ContainerBuilder"/>.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandTreeItemViewMapper>().As<ICommandTreeItemViewMapper>().SingleInstance();
            var logger = new LogViewModel();
            var tokenClient = new TokenClient(HttpMessagesViewModel.Instance);
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterInstance(tokenClient).As<ITokenClient>();
        }
    }
}
