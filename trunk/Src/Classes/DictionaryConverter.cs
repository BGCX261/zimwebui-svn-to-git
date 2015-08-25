using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Text.RegularExpressions;
namespace Telerik.Samples
{
    /// <summary>
    /// This class implements methods that convert string representation of Dictionary
    /// to Dictionary object and vice versa - converter will convert only dictionaries
    /// where both key and value are of type string
    /// </summary>
    public class DictionaryConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to
        /// the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">A System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="sourceType">A System.Type that represents the type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type,
        /// using the specified context.
        /// </summary>
        /// <param name="context">A System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="destinationType">A System.Type that represents the type you want to convert to.</param>
        /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }
        /// <summary>
        /// Converts the given object to the type of this converter, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">A System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The System.Globalization.CultureInfo to use as the current culture.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        /// <exception cref="System.NotSupportedException">The conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            // the logic in this method will convert string into a Dictionary. The string has following
            // format:
            // key1=value1;key2=value2
            // where semi-color (;) separates dictionary items, while equal sign (=) separates
            // item key from item value
            if (value is string)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                // split string by semi-colon (;) to get array of dictionary items
                string[] dictionaryItems = Regex.Split((string)value, "::");
                foreach (string dictionaryItem in dictionaryItems)
                {
                    // split item by equal sign (=) to get array of key and value
                    string[] keyValuePair = Regex.Split(dictionaryItem, "==="); 
                    // add new item to the dictionary
                    if (keyValuePair.Length == 2)
                        dictionary.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                }
                return dictionary;
            }
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// Converts the given value object to the specified type, using the specified
        ///     context and culture information.
        /// </summary>
        /// <param name="context">A System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">A System.Globalization.CultureInfo. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <param name="destinationType">The System.Type to convert the value parameter to.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        /// <exception cref="System.NotSupportedException">The conversion cannot be performed.</exception>
        /// <exception cref="System.ArgumentNullException">The destinationType parameter is null.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // the logic in this method will convert Dictionary into a string (serialize the dictionary).
            //The string has following format:
            // key1=value1;key2=value2
            // where semi-color (;) separates dictionary items, while equal sign (=) separates
            // item key from item value
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");
            if ((destinationType == typeof(string)) && (value is Dictionary<string, string>))
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> dictionaryItem in (Dictionary<string, string>)value)
                {
                    sb.AppendFormat("{0}==={1}::", dictionaryItem.Key, dictionaryItem.Value);
                }
                return sb.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}