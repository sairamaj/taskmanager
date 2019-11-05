using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Utils.Core.Extensions;
using Utils.Core.Model;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Tree view for generic object.
    /// </summary>
    public class ObjectTreeViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Object depth.
        /// </summary>
        private readonly int _depth;

        /// <summary>
        /// Name of the object.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Object value.
        /// </summary>
        private readonly object _obj;

        /// <summary>
        /// Object extracting information type.
        /// </summary>
        private readonly InfoType _infoType;

        /// <summary>
        /// Indicates the object has only one property.
        /// </summary>
        private bool _isOneProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectTreeViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// Parent object.
        /// </param>
        /// <param name="name">
        /// Name of the object.
        /// </param>
        /// <param name="obj">
        /// Object value.
        /// </param>
        /// <param name="infoType">
        /// Extracting information.
        /// </param>
        public ObjectTreeViewModel(TreeViewItemViewModel parent, string name, object obj, InfoType infoType)
            : this(parent, name, obj, infoType, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectTreeViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// Parent object.
        /// </param>
        /// <param name="name">
        /// Name of the object.
        /// </param>
        /// <param name="obj">
        /// Object value.
        /// </param>
        /// <param name="infoType">
        /// Extracting information.
        /// </param>
        /// <param name="depth">
        /// Depth to be traversed.
        /// </param>
        private ObjectTreeViewModel(TreeViewItemViewModel parent, string name, object obj, InfoType infoType, int depth)
            : base(parent, name, true)
        {
            this._name = name;
            this._obj = obj;
            this._infoType = infoType;
            this._depth = depth;
            this.Title = this.GetTitle(this._obj);
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets object value.
        /// </summary>
        public object Object => this._obj;

        /// <summary>
        /// Gets or sets name property of the object.
        /// </summary>
        public override string Name
        {
            get
            {
                if (this._obj == null)
                {
                    return null;
                }

                if (this._name != null)
                {
                    return $"{this._name}{this.Title}";
                }

                return $"{this.Title}";
            }
            set => base.Name = value;
        }

        /// <summary>
        /// Load object inner properties or fields.
        /// </summary>
        protected override void LoadChildren()
        {
            if (this._isOneProperty)
            {
                return;
            }

            // todo: why 2?
            if (this._depth > 2)
            {
                return;
            }

            if (this._obj == null || this._obj.GetType() == typeof(string) || this._obj.GetType().IsValueType)
            {
                return;
            }

            if (typeof(IDictionary).IsAssignableFrom(this._obj.GetType()))
            {
                var dictionary = this._obj as IDictionary;
                if (dictionary == null)
                {
                    return;
                }

                foreach (var key in dictionary.Keys)
                {
                    this.Children.Add(new ObjectTreeViewModel(this, key.ToString(), dictionary[key], this._infoType, this._depth + 1));
                }

                return;
            }

            if (typeof(IList).IsAssignableFrom(this._obj.GetType()))
            {
                if (this._obj is IList list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : $"[{i}]", list[i], this._infoType, this._depth + 1));
                    }
                }

                return;
            }

            if (this._infoType == InfoType.Properties || this._infoType == InfoType.Both)
            {
                foreach (var prop in this._obj.GetType().GetProperties().OrderBy(p => p.Name))
                {
                    try
                    {
                        var objValue = prop.GetValue(this._obj, null);
                        if (objValue == null)
                        {
                            continue;
                        }

                        if (objValue is IList)
                        {
                            var list = objValue as IList;
                            for (int i = 0; i < list.Count; i++)
                            {
                                this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : $"[{i}]", list[i], this._infoType, this._depth + 1));
                            }
                        }
                        else if (objValue is IDictionary)
                        {
                            // MessageBox.Show(String.Format("Ignoring IDictionary for property:{0}",prop.Name));
                        }
                        else if (objValue.GetType() == typeof(ExtensionDataObject))
                        {
                            // ignore this as this is very common for wcf responss.
                        }
                        else
                        {
                            var objectItem = new NameValueTreeViewModel(this, prop.Name, objValue.ToString());
                            this.Children.Add(objectItem);

                            if (prop.PropertyType.GetInterface("IEnumerable`1") != null &&
                                prop.PropertyType.IsGenericType)
                            {
                                var genericTypeArgument = prop.PropertyType.GetGenericArguments()[0];
                                foreach (var item in (IEnumerable)objValue)
                                {
                                    objectItem.Children.Add(new ObjectTreeViewModel(this, this.GetTitle(item), item, InfoType.Properties, this._depth + 1));
                                }
                            }
                            else if (!objValue.GetType().IsValueType && objValue.GetType() != typeof(string))
                            {
                                objectItem.Children.Add(
                                    new ObjectTreeViewModel(this, objValue.ToString(), objValue, this._infoType, this._depth + 1));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"Exception {e}");
                        return;
                    }
                }
            }

            if (this._infoType == InfoType.Fields || this._infoType == InfoType.Both)
            {
                foreach (var field in this._obj.GetType().GetFields().OrderBy(f => f.Name))
                {
                    var objValue = field.GetValue(this._obj);

                    if (objValue is IList list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : $"[{i}]", list[i], this._infoType, this._depth + 1));
                        }
                    }
                    else if (!field.FieldType.IsClass || field.FieldType == typeof(string) || objValue == null)
                    {
                        this.Children.Add(new NameValueTreeViewModel(this, field.Name, objValue == null ? string.Empty : objValue.ToString()));
                    }
                    else
                    {
                        this.Children.Add(new ObjectTreeViewModel(this, field.Name, objValue, InfoType.Both));
                    }
                }
            }
        }

        private string GetTitle(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }

            if (obj.GetType().IsValueType)
            {
                return obj.ToString();
            }

            // try to see whether name property
            var nameProperty = obj.GetType().GetProperty("Name");
            if (nameProperty != null)
            {
                var namePropertyValue = nameProperty.GetValue(obj, null);
                if (namePropertyValue != null)
                {
                    return namePropertyValue.ToString();
                }
            }

            var typeName = obj.GetType().Name;
            nameProperty = obj.GetType().GetProperty(typeName + "Name");    // try to see whether "<typeName>Name" ( for ex: SubCategory type SubCategoryName)
            if (nameProperty != null)
            {
                var namePropertyValue = nameProperty.GetValue(obj, null);
                if (namePropertyValue != null)
                {
                    return namePropertyValue.ToString();
                }
            }

            // If there is only one property and is value type(or string )then use it
            var val = obj.TryForOneProperty();
            if (val != null)
            {
                this._isOneProperty = true;
                return $"{typeName} ({val})";
            }

            return typeName;
        }
    }
}
