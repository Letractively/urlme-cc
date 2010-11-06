using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace urlme.Core.Enum
{
    public sealed class StringAttribute
    {
        private Type enumType;
        private static Hashtable stringValues = new Hashtable();

        #region Properties

        public Type EnumType
        {
            get
            {
                return enumType;
            }
        }

        #endregion

        #region Constructor

        public StringAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", enumType.ToString()));

            this.enumType = enumType;
        }

        #endregion

        #region StringValue

        public string StringValue(System.Enum enumValue)
        {
            if (enumValue.GetType() != this.enumType)
                throw new ArgumentException(String.Format("Supplied value must be an Enum of type {0}.  Type was {0}", enumType.ToString(), enumValue.GetType()));

            string stringValue = null;
            try
            {
                stringValue = GetStringValue(enumValue);
            }
            catch (Exception)
            { /* silent */
            }

            return stringValue;
        }

        #endregion

        #region StringValues

        public List<string> StringValues
        {
            get
            {
                List<string> values = new List<string>();
                foreach (FieldInfo fi in enumType.GetFields())
                {
                    //Check for our custom attribute
                    StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                        values.Add(attrs[0].Value);
                }
                return values;
            }
        }

        #endregion

        #region ListValues

        public IList ListValues
        {
            get
            {
                Type underlyingType = System.Enum.GetUnderlyingType(enumType);
                ArrayList values = new ArrayList();
                //Look for our string value associated with fields in this enum
                foreach (FieldInfo fi in enumType.GetFields())
                {
                    //Check for our custom attribute
                    StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                        values.Add(new DictionaryEntry(Convert.ChangeType(System.Enum.Parse(enumType, fi.Name), underlyingType), attrs[0].Value));

                }
                return values;
            }
        }

        #endregion

        #region StaticItems

        #region GetStringValue

        public static string GetStringValue(System.Enum typeValue)
        {
            string output = null;
            Type type = typeValue.GetType();

            if (stringValues.ContainsKey(typeValue))
            {
                output = (stringValues[typeValue] as StringValueAttribute).Value;
            }
            else
            {
                // Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(typeValue.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;

                    lock (stringValues)
                    {
                        if (!stringValues.ContainsKey(typeValue))
                        {
                            stringValues.Add(typeValue, attrs[0]);
                        }
                    }
                }
            }

            return output;
        }

        #endregion

        #region Parse

        public static T Parse<T>(string stringValue)
        {
            return Parse<T>(stringValue, false);
        }

        public static T Parse<T>(string stringValue, bool ignoreCase)
        {
            Type type = typeof(Type);
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = System.Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return (T)output;
        }

        #endregion

        #region IsStringDefined

        public static bool IsStringDefined<T>(string stringValue)
        {
            return Parse<T>(stringValue) != null;
        }

        public static bool IsStringDefined<T>(string stringValue, bool ignoreCase)
        {
            return Parse<T>(stringValue, ignoreCase) != null;
        }

        #endregion

        #region ListValues

        public static IList GetNames(Type type)
        {
            List<string> values = new List<string>();
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    values.Add(attrs[0].Value);
            }
            return values;
        }

        #endregion

        #endregion
    }

    public class StringValueAttribute : Attribute
    {
        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }
    }
}