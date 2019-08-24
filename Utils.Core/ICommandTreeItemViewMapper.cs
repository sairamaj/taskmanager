using System;
using System.Windows.Controls;

namespace Utils.Core
{
    /// <summary>
    /// The CommandTreeItemViewMapper interface.
    /// </summary>
    public interface ICommandTreeItemViewMapper
    {
        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        void Add(string tag, UserControl view);

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        void Remove(string tag);

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <returns>
        /// The <see cref="UserControl"/>.
        /// </returns>
        UserControl GetControl(string tag);

        /// <summary>
        /// The add UI template.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        void AddUiTemplate(string name, Type type);

        /// <summary>
        /// The get UI template.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        Type GetUiTemplate(string name);
    }
}
