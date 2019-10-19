using System;
using System.Collections;
using System.Linq;

namespace Utils.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetPropertyValue(this object obj, string propName)
        {
            if (obj == null)
                return null;
            var prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                var val = prop.GetValue(obj, null);
                if (val != null)
                    return val.ToString();
            }
            return null;
        }

        public static object GetObjectValueByPartialName(this object obj, string partialName)
        {
            if (obj == null || obj is IList)
                return null;

            var lowerPartialName = partialName.ToLower();
            // Is it in Property
            var prop = obj.GetType().GetProperties().ToList().FirstOrDefault(p => p.Name.ToLower().Contains(lowerPartialName));
            if (prop != null)
            {
                return prop.GetValue(obj, null);
            }

            // Is in Field
            var field = obj.GetType().GetFields().ToList().FirstOrDefault(f => f.Name.ToLower().Contains(lowerPartialName));
            if (field != null)
                return field.GetValue(obj);

            // Look in all reference properties
            foreach (var refProp in obj.GetType().GetProperties().ToList().Where(p => IsTypeNeedsToBeConsideredForDeepValue(p.PropertyType)))
            {
                var refValue = refProp.GetValue(obj, null);
                var val = GetObjectValueByPartialName(refValue, partialName);
                if (val != null)
                    return val.ToString();
            }
            // Look in all reference fields.

            foreach (var refField in obj.GetType().GetFields().ToList().Where(f => IsTypeNeedsToBeConsideredForDeepValue(f.FieldType)))
            {
                var refValue = refField.GetValue(obj);
                var val = GetObjectValueByPartialName(refValue, partialName);
                if (val != null)
                    return val.ToString();
            }
            return null;
        }

        public static string TryForOneProperty(this object obj)
        {
            if (obj == null)
                return null;

            if (obj.GetType().GetProperties().Length + obj.GetType().GetFields().Length != 1)
                return null;
            if (obj.GetType().GetProperties().Length == 1)
            {
                var oneProperty = obj.GetType().GetProperties().ToList().First();
                if (oneProperty.PropertyType.IsValueType || oneProperty.PropertyType == typeof(string))
                {
                    var val = oneProperty.GetValue(obj, null);
                    if (val != null)
                        return String.Format("{0}-{1}", oneProperty.Name, val.ToString());
                }
            }
            else
            {
                var oneField = obj.GetType().GetFields().ToList().First();
                if (oneField.FieldType.IsValueType || oneField.FieldType == typeof(string))
                {
                    var val = oneField.GetValue(obj);
                    if (val != null)
                        return String.Format("{0}-{1}", oneField.Name, val.ToString());
                }
            }
            return null;
        }

        static bool IsTypeNeedsToBeConsideredForDeepValue(Type type)
        {
            return type != typeof(string) && !type.IsValueType && (type != typeof(System.Runtime.Serialization.ExtensionDataObject));
        }
    }
}
