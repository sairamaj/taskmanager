using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Threading;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The view model base.
    /// </summary>
    public abstract class CoreViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The system events.
        /// </summary>
        private static readonly EventHandler<ViewModelEventArgs> SystemEvents = (sender, args) => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreViewModel"/> class.
        /// </summary>
        protected CoreViewModel()
        {
            this.UiDispatcher = Application.Current.Dispatcher;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the UI dispatcher.
        /// </summary>
        /// <value>UI dispatcher.</value>
        protected Dispatcher UiDispatcher { get; }

        /// <summary>
        /// Execute given action in UI thread.
        /// </summary>
        /// <param name="action">
        /// Action to be executed.
        /// </param>
        public static void ExecuteAsync(Action action)
        {
            if (Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.InvokeAsync(new Action(action), DispatcherPriority.ContextIdle);
            }
        }

        /// <summary>
        /// On activate.
        /// </summary>
        public virtual void OnActivate()
        {
        }

        /// <summary>
        /// The get drag data.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Copied code from some other project.")]
        public virtual object GetDragData()
        {
            return null;
        }

        /// <summary>
        /// The on drop.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public virtual void OnDrop(Point point, IDataObject data)
        {
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public virtual void Refresh()
        {
        }

        /// <summary>
        /// The get type sample value.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected static string GetTypeSampleValue(Type type)
        {
            if (type == null)
            {
                return String.Empty;
            }

            if (type == typeof(string))
            {
                return @"""some value""";
            }

            if (type == typeof(bool))
            {
                return "false";
            }

            if (type == typeof(Guid))
            {
                return @"Guid.NewGuid()";
            }

            if (type == typeof(Uri))
            {
                return @"new Uri(""http:/uri"")";
            }

            if (type.IsClass)
            {
                return String.Format(CultureInfo.InvariantCulture, "new {0}();", type);
            }

            if (type.IsEnum)
            {
                var val = Enum.GetNames(type).FirstOrDefault();
                if (val != null)
                {
                    return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.FullName, val);
                }

                return "n/a";
            }

            return "100";
        }

        /// <summary>
        /// The is system type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="type"/> is <see language="null" />.
        /// </exception>
        protected static bool IsSystemType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.Assembly.FullName.Contains("mscorlib"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The ignore exception.
        /// </summary>
        /// <param name="action">
        /// The a.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="action"/> is <see language="null"/>.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Method should ignore the exception.")]
        protected static void IgnoreException(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action();
            }
            catch (Exception e)
            {
                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, "Action :{0} with ignore exception got:{1}", action, e));
            }
        }

        /// <summary>
        /// The show error.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected static void ShowError(string format, params object[] args)
        {
            MessageBox.Show(String.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// The on property changed 2.
        /// </summary>
        /// <param name="selectorExpression">
        /// The selector expression.
        /// </param>
        /// <typeparam name="T">Type name.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="selectorExpression"/> is <see language="null"/>.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Copied code from some other project.")]
        protected void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
            {
                throw new ArgumentNullException(nameof(selectorExpression));
            }

            var body = selectorExpression.Body as MemberExpression;
            if (body == null)
            {
                return;
            }

            var propertyName = body.Member.Name;

            PropertyChangedEventHandler handler = this.PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The send system event.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        protected void SendSystemEvent(string name, object data)
        {
            SystemEvents(this, new ViewModelEventArgs(name, data));
        }

        /// <summary>
        /// The execute in UI thread.
        /// </summary>
        /// <param name="func">
        /// The function.
        /// </param>
        /// <typeparam name="T">Type name.
        /// </typeparam>
        /// <returns>
        /// The Type name.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "InUi", Justification = "Copied code from some other project.")]
        protected T ExecuteInUiThread<T>(Func<T> func)
        {
            return this.UiDispatcher.Execute<T>(func);
        }
    }
}