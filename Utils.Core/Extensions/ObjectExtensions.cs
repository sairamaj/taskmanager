using System;
using System.Collections;
using System.Linq;

namespace Utils.Core.Extensions
{
    /// <summary>
    /// Object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="obj">
        /// Object value.
        /// </param>
        /// <param name="propName">
        /// Property name.
        /// </param>
        /// <returns>
        /// Property value.
        /// </returns>
        public static string GetPropertyValue(this object obj, string propName)
        {
            if (obj == null)
            {
                return null;
            }

            var prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                var val = prop.GetValue(obj, null);
                if (val != null)
                {
                    return val.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Gets object value.
        /// </summary>
        /// <param name="obj">
        /// Object value.
        /// </param>
        /// <param name="partialName">
        /// Name of the property.
        /// </param>
        /// <returns>
        /// Property value.
        /// </returns>
        public static object GetObjectValueByPartialName(this object obj, string partialName)
        {
            if (obj == null || obj is IList)
            {
                return null;
            }

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
            {
                return field.GetValue(obj);
            }

            // Look in all reference properties
            foreach (var refProp in obj.GetType().GetProperties().ToList().Where(p => IsTypeNeedsToBeConsideredForDeepValue(p.PropertyType)))
            {
                var refValue = refProp.GetValue(obj, null);
                var val = GetObjectValueByPartialName(refValue, partialName);
                if (val != null)
                {
                    return val.ToString();
                }
            }

            // Look in all reference fields.
            foreach (var refField in obj.GetType().GetFields().ToList().Where(f => IsTypeNeedsToBeConsideredForDeepValue(f.FieldType)))
            {
                var refValue = refField.GetValue(obj);
                var val = GetObjectValueByPartialName(refValue, partialName);
                if (val != null)
                {
                    return val.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Try for one property value.
        /// </summary>
        /// <param name="obj">
        /// Object value..
        /// </param>
        /// <returns>
        /// Property value.
        /// </returns>
        public static string TryForOneProperty(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.GetType().GetProperties().Length + obj.GetType().GetFields().Length != 1)
            {
                return null;
            }

            if (obj.GetType().GetProperties().Length == 1)
            {
                var oneProperty = obj.GetType().GetProperties().ToList().First();
                if (oneProperty.PropertyType.IsValueType || oneProperty.PropertyType == typeof(string))
                {
                    var val = oneProperty.GetValue(obj, null);
                    if (val != null)
                    {
                        return $"{oneProperty.Name}-{val.ToString()}";
                    }
                }
            }
            else
            {
                var oneField = obj.GetType().GetFields().ToList().First();
                if (oneField.FieldType.IsValueType || oneField.FieldType == typeof(string))
                {
                    var val = oneField.GetValue(obj);
                    if (val != null)
                    {
                        return $"{oneField.Name}-{val.ToString()}";
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks whether type need to be extracted.
        /// </summary>
        /// <param name="type">
        /// Type name.
        /// </param>
        /// <returns>
        /// true if type needs to be considered for traversing.
        /// </returns>
        private static bool IsTypeNeedsToBeConsideredForDeepValue(Type type)
        {
            return type != typeof(string) && !type.IsValueType && (type != typeof(System.Runtime.Serialization.ExtensionDataObject));
        }
    }
}
