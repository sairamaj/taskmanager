using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    public class ObjectTreeViewModel : TreeViewItemViewModel
    {
        private readonly int _depth;

        public enum InfoType
        {
            Properties,
            Fields,
            Both
        }

        private readonly string _name;
        private readonly object _obj;
        private readonly InfoType _infoType;
        public string Title { get; set; }
        bool _isOneProperty;
        private ObjectTreeViewModel(TreeViewItemViewModel parent, string name, object obj, InfoType infoType, int depth)
            : base(parent, name, true)
        {
            _name = name;
            _obj = obj;
            _infoType = infoType;
            _depth = depth;
            Title = GetTitle(_obj);
            IsExpanded = true;
        }

        public ObjectTreeViewModel(TreeViewItemViewModel parent, string name, object obj, InfoType infoType)
            : this(parent, name, obj, infoType, 0)
        {
        }

        public object Object { get { return _obj; } }

        public override string Name
        {
            get
            {
                if (_obj == null)
                    return null;
                if (_name != null)
                {
                    return String.Format("{0}{1}", _name, Title);
                }
                else
                {
                    return String.Format("{0}", Title);
                }
            }
            set
            {
                base.Name = value;
            }
        }


        protected override void LoadChildren()
        {
            if (_isOneProperty)
                return;
            if (_depth > 4)
                return;
            if (_obj == null || _obj.GetType() == typeof(string) || _obj.GetType().IsValueType)
                return;
            if (typeof(IDictionary).IsAssignableFrom(_obj.GetType()))
            {
                var dictionary = _obj as IDictionary;
                if (dictionary == null)
                    return;
                foreach (var key in dictionary.Keys)
                {
                    this.Children.Add(new ObjectTreeViewModel(this, key.ToString(), dictionary[key], _infoType, _depth + 1));
                }
                return;
            }
            else if (typeof(IList).IsAssignableFrom(_obj.GetType()))
            {
                var list = _obj as IList;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : String.Format("[{0}]", i), list[i], _infoType, _depth + 1));
                    }
                }
                return;
            }
            if (_infoType == InfoType.Properties || _infoType == InfoType.Both)
            {
                foreach (var prop in _obj.GetType().GetProperties().OrderBy(p => p.Name))
                {
                    try
                    {
                        var objValue = prop.GetValue(_obj, null);
                        if (objValue == null)
                            continue;
                        if (objValue is IList)
                        {
                            var list = objValue as IList;
                            for (int i = 0; i < list.Count; i++)
                            {
                                this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : String.Format("[{0}]", i), list[i], _infoType, _depth + 1));
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

                            var objectItem = new NameValueTreeViewModel(this, prop.Name,
                                                                        objValue == null ? "" : objValue.ToString());
                            this.Children.Add(objectItem);

                            if (prop.PropertyType.GetInterface("IEnumerable`1") != null &&
                                prop.PropertyType.IsGenericType &&
                                objValue != null)
                            {
                                var genericTypeArgument = prop.PropertyType.GetGenericArguments()[0];
                                foreach (var item in objValue as IEnumerable)
                                {
                                    objectItem.Children.Add(new ObjectTreeViewModel(this, GetTitle(item), item,
                                                                                    InfoType.Properties, _depth + 1));
                                }
                            }
                            else if (objValue != null && !objValue.GetType().IsValueType && objValue.GetType() != typeof(string))
                            {
                                objectItem.Children.Add(new ObjectTreeViewModel(this, objValue.ToString(), objValue,
                                                                                _infoType, _depth + 1));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //   MessageBox.Show(String.Format("Property:{0} of type:{1} excpetion:{2} obj:{3} objGetType:{4}", prop.Name,
                        //                               prop.PropertyType, e.Message, _obj, _obj.GetType()));
                        //throw;
                        return;
                    }
                }
            }

            if (_infoType == InfoType.Fields || _infoType == InfoType.Both)
            {
                foreach (var field in _obj.GetType().GetFields().OrderBy(f => f.Name))
                {
                    var objValue = field.GetValue(_obj);

                    if (objValue is IList)
                    {
                        var list = objValue as IList;
                        for (int i = 0; i < list.Count; i++)
                        {
                            this.Children.Add(new ObjectTreeViewModel(this, list[i] == null ? "(null)" : String.Format("[{0}]", i), list[i], _infoType, _depth + 1));
                        }
                    }
                    else if (!field.FieldType.IsClass || field.FieldType == typeof(string) || objValue == null)
                    {
                        this.Children.Add(new NameValueTreeViewModel(this, field.Name,
                                                                 objValue == null ? "" : objValue.ToString()));
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
                return "null";
            if (obj.GetType() == typeof(string))
                return obj.ToString();
            if (obj.GetType().IsValueType)
                return obj.ToString();

            var nameProperty = obj.GetType().GetProperty("Name");    // try to see whether name property 
            if (nameProperty != null)
            {
                var namePropertyValue = nameProperty.GetValue(obj, null);
                if (namePropertyValue != null)
                    return namePropertyValue.ToString();
            }

            var typeName = obj.GetType().Name;
            nameProperty = obj.GetType().GetProperty(typeName + "Name");    // try to see whether "<typeName>Name" ( for ex: SubCategory type SubCategoryName)
            if (nameProperty != null)
            {
                var namePropertyValue = nameProperty.GetValue(obj, null);
                if (namePropertyValue != null)
                    return namePropertyValue.ToString();
            }

            // If there is only one property and is value type(or string )then use it
            var val = obj.TryForOneProperty();
            if (val != null)
            {
                _isOneProperty = true;
                return String.Format("{0} ({1})", typeName, val);
            }
            return typeName;
        }
    }
}
