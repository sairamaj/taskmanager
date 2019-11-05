using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Utils.Core.Command;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The tree view item view model.
    /// </summary>
    public class TreeViewItemViewModel : CoreViewModel
    {
        /// <summary>
        /// The dummy child.
        /// </summary>
        private static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        /// <summary>
        /// The children.
        /// </summary>
        private readonly ObservableCollection<TreeViewItemViewModel> _children;

        /// <summary>
        /// The parent.
        /// </summary>
        private readonly TreeViewItemViewModel _parent;

        /// <summary>
        /// The success flag.
        /// </summary>
        private bool? _isSuccess;

        /// <summary>
        /// The working flag.
        /// </summary>
        private bool _isWorking;

        /// <summary>
        /// The name.
        /// </summary>
        private string _name;

        /// <summary>
        /// The _is expanded.
        /// </summary>
        private bool _isExpanded;

        /// <summary>
        /// The _is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// The work in progress command view.
        /// </summary>
        private WorkInProgressCommandTreeViewModel _workinProgressCommandView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="lazyLoadChildren">
        /// The lazy load children.
        /// </param>
        protected TreeViewItemViewModel(TreeViewItemViewModel parent, string name, bool lazyLoadChildren)
        {
            this._parent = parent;
            this._name = name;

            this._children = new SafeObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
            {
                this._children.Add(DummyChild);
            }

            this.Children.CollectionChanged += (s, e) =>
            {
                if (this.ChildAdded != null)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        this.ChildAdded(this, new ChildChangedEventArgs(e.NewItems[e.NewItems.Count - 1] as TreeViewItemViewModel));
                    }
                }
            };

            this.ExpandingCommand = new DelegateCommand(() => { this.IsExpanded = true; });
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TreeViewItemViewModel"/> class from being created.
        /// </summary>
        private TreeViewItemViewModel()
        {
        }

        /// <summary>
        /// Gets or sets child added event.
        /// </summary>
        /// <value>Child added event handler.</value>
        public EventHandler<ChildChangedEventArgs> ChildAdded { get; set; }

        /// <summary>
        /// Gets or sets the expanding command.
        /// </summary>
        /// <value>Expand command.</value>
        public ICommand ExpandingCommand { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>Children items.</value>
        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get
            {
                if (this._children == null)
                {
                    return new SafeObservableCollection<TreeViewItemViewModel>();
                }

                return this._children;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has dummy child.
        /// </summary>
        /// <value>Dummy child flag..</value>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is expanded.
        /// </summary>
        /// <value>Is expanded flag.</value>
        public bool IsExpanded
        {
            get
            {
                return this._isExpanded;
            }

            set
            {
                if (value != this._isExpanded)
                {
                    this._isExpanded = value;
                    this.OnPropertyChanged(() => this.IsExpanded);
                }

                // Expand all the way up to the root.
                if (this._isExpanded && this._parent != null)
                {
                    this._parent.IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        /// <value>Selected flag.</value>
        public virtual bool IsSelected
        {
            get
            {
                return this._isSelected;
            }

            set
            {
                if (value != this._isSelected)
                {
                    this._isSelected = value;
                    this.OnPropertyChanged(() => this.IsSelected);
                }
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>Parent tree item.</value>
        public TreeViewItemViewModel Parent
        {
            get
            {
                return this._parent;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Name of the node.</value>
        public virtual string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// Gets or sets the is success.
        /// </summary>
        /// <value>Success flag.</value>
        public virtual bool? IsSuccess
        {
            get => this._isSuccess;

            set
            {
                this._isSuccess = value;
                this.OnPropertyChanged(() => this.IsSuccess);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is working.
        /// </summary>
        /// <value>Working flag.</value>
        public bool IsWorking
        {
            get => this._isWorking;

            set
            {
                this._isWorking = value;
                this.OnPropertyChanged(() => this.IsWorking);
            }
        }

        /// <summary>
        /// The collapse all.
        /// </summary>
        public void CollapseAll()
        {
            this.ApplyActionToAllItems(item => item.IsExpanded = false);
        }

        /// <summary>
        /// The expand all.
        /// </summary>
        public void ExpandAll()
        {
            this.ApplyActionToAllItems(item => item.IsExpanded = true);
        }

        /// <summary>
        /// The add work in progress.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void AddWorkInProgress(string message)
        {
            this.IsWorking = true;
            this._workinProgressCommandView = new WorkInProgressCommandTreeViewModel(message);
            this.Children.Add(this._workinProgressCommandView);
        }

        /// <summary>
        /// The add work in progress in UI thread.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="WorkInProgressCommandTreeViewModel"/>.
        /// </returns>
        public WorkInProgressCommandTreeViewModel AddWorkInProgressInUIThread(string message)
        {
            var workinProgressCommandView = new WorkInProgressCommandTreeViewModel(message);
            this.UiDispatcher.Execute(() => this.Children.Add(workinProgressCommandView));
            return workinProgressCommandView;
        }

        /// <summary>
        /// The remove work in progress.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        public void RemoveWorkInProgress(WorkInProgressCommandTreeViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            viewModel.MarkWorkDone();
            this.IsWorking = false;
            this.UiDispatcher.Execute(() => this.Children.Remove(viewModel));
        }

        /// <summary>
        /// The remove work in progress.
        /// </summary>
        public void RemoveWorkInProgress()
        {
            if (this._workinProgressCommandView == null)
            {
                return;
            }

            this._workinProgressCommandView.MarkWorkDone();
            this.UiDispatcher.Execute(() =>
            {
                this.Children.Remove(this._workinProgressCommandView);
                this._workinProgressCommandView = null;
            });
        }

        /// <summary>
        /// The add to children in UI thread.
        /// </summary>
        /// <param name="child">
        /// The child.
        /// </param>
        public void AddToChildrenInUIThread(TreeViewItemViewModel child)
        {
            this.UiDispatcher.Execute(() => this.Children.Add(child));
        }

        /// <summary>
        /// The add to children in UI thread.
        /// </summary>
        /// <param name="childs">
        /// The childs.
        /// </param>
        public void AddToChildrenInUIThread(IEnumerable<TreeViewItemViewModel> childs)
        {
            this.UiDispatcher.Execute(() =>
            {
                foreach (var child in childs)
                {
                    this.Children.Add(child);
                }
            });
        }

        /// <summary>
        /// The add error view model.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void AddErrorViewModel(string message, Exception exception)
        {
            var child = new ErrorInfoViewModel(message, exception);
            this.AddToChildrenInUIThread(child);
        }

        /// <summary>
        /// The add error view model.
        /// </summary>
        /// <param name="errorViewModel">
        /// The error view model.
        /// </param>
        public void AddErrorViewModel(ErrorInfoViewModel errorViewModel)
        {
            this.AddToChildrenInUIThread(errorViewModel);
        }

        /// <summary>
        /// The load children async.
        /// </summary>
        /// <param name="dataRetriever">
        /// The data retriever.
        /// </param>
        /// <param name="childNodeCreator">
        /// The child node creator.
        /// </param>
        /// <typeparam name="T">Type name.
        /// </typeparam>
        protected virtual void LoadChildrenAsync<T>(Func<IEnumerable<T>> dataRetriever, Func<T, TreeViewItemViewModel> childNodeCreator)
        {
            this.LoadChildrenAsync(dataRetriever, childNodeCreator, null);
        }

        /// <summary>
        /// The load children async.
        /// </summary>
        /// <param name="dataRetriever">
        /// The data retriever.
        /// </param>
        /// <param name="childNodeCreator">
        /// The child node creator.
        /// </param>
        /// <param name="afterLoadingAction">
        /// The after loading action.
        /// </param>
        /// <typeparam name="T">The type name.
        /// </typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Used in UI.")]
        protected virtual void LoadChildrenAsync<T>(Func<IEnumerable<T>> dataRetriever, Func<T, TreeViewItemViewModel> childNodeCreator, Action afterLoadingAction)
        {
            this.AddWorkInProgress("Retrieving...");
            ThreadPool.QueueUserWorkItem(w =>
            {
                try
                {
                    var childs = new List<TreeViewItemViewModel>();
                    foreach (T data in dataRetriever())
                    {
                        childs.Add(childNodeCreator(data));
                    }

                    this.AddToChildrenInUIThread(childs);
                }
                catch (Exception e)
                {
                    this.AddToChildrenInUIThread(new ExceptionTreeViewItemViewModel(null, e));
                }
                finally
                {
                    if (afterLoadingAction != null)
                    {
                        try
                        {
                            afterLoadingAction();
                        }
                        catch (Exception e)
                        {
                            // ignore here as we don't want to raise exception from background thread.
                            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "TreeViewItemViewModel.LoadChildrenAsync exception:{0}", e));
                        }
                    }

                    this.RemoveWorkInProgress();
                }
            });
        }

        /// <summary>
        /// The get children.
        /// </summary>
        /// <typeparam name="T">The type name.
        /// </typeparam>
        /// <returns>
        /// The <see cref="System.Collections.IList"/>.
        /// </returns>
        protected IList<T> GetChildren<T>()
        {
            if (this.Children == null)
            {
                return new List<T>();
            }

            var items = new List<T>(this.Children.OfType<T>().ToList());
            this.Children.ToList().ForEach(c1 => c1.GetChildren<T>().ToList().ForEach(items.Add));
            return items;
        }

        /// <summary>
        /// The get children.
        /// </summary>
        /// <param name="isSuccess">
        /// The is success.
        /// </param>
        /// <returns>
        /// The <see cref="System.Collections.IList"/>.
        /// </returns>
        protected IList<TreeViewItemViewModel> GetChildren(bool isSuccess)
        {
            if (this.Children == null)
            {
                return new List<TreeViewItemViewModel>();
            }

            var items = new List<TreeViewItemViewModel>(this.Children.Where(t => t.IsSuccess == isSuccess));
            this.Children.ToList().ForEach(c1 => c1.GetChildren(isSuccess).ToList().ForEach(items.Add));
            return items;
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        /// <summary>
        /// This helper method traverses the tree in a depth-first non-recursive way
        /// and executes the action passed as a parameter on each item.
        /// </summary>
        /// <param name="itemAction">Action to be executed for each item.</param>
        private void ApplyActionToAllItems(Action<TreeViewItemViewModel> itemAction)
        {
            var dataItemStack = new Stack<TreeViewItemViewModel>();
            dataItemStack.Push(this);

            while (dataItemStack.Count != 0)
            {
                TreeViewItemViewModel currentItem = dataItemStack.Pop();
                itemAction(currentItem);
                foreach (TreeViewItemViewModel childItem in currentItem.Children)
                {
                    dataItemStack.Push(childItem);
                }
            }
        }
    }
}
