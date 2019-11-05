using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Utils.Core
{
    /// <summary>
    /// The command tree item view mapper.
    /// </summary>
    public class CommandTreeItemViewMapper : ICommandTreeItemViewMapper
    {
        /// <summary>
        /// The user controls.
        /// </summary>
        private readonly IDictionary<string, UserControl> _userControls = new Dictionary<string, UserControl>();

        /// <summary>
        /// The user control templates.
        /// </summary>
        private readonly IDictionary<string, Type> _userControlTemplates = new Dictionary<string, Type>();

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        public void Add(string tag, UserControl view)
        {
            this._userControls[tag] = view;
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        public void Remove(string tag)
        {
            if (this._userControls.ContainsKey(tag))
            {
                this._userControls.Remove(tag);
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <returns>
        /// The <see cref="UserControl"/>.
        /// </returns>
        public UserControl GetControl(string tag)
        {
            return this._userControls.ContainsKey(tag) ? this._userControls[tag] : null;
        }

        /// <summary>
        /// The add UI template.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        public void AddUiTemplate(string name, Type type)
        {
            this._userControlTemplates[name] = type;
        }

        /// <summary>
        /// The get UI template.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>Template type.
        /// </returns>
        public Type GetUiTemplate(string name)
        {
            if (this._userControlTemplates.ContainsKey(name))
            {
                return this._userControlTemplates[name];
            }

            return null;
        }
    }
}