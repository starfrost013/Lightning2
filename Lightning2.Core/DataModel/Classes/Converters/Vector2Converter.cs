using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Vector2Converter [Non-DataModel]
    /// 
    /// April 11, 2021
    /// 
    /// Converts strings to Vector2 for DDMS attributes 
    /// </summary>
    [TypeConverter]
    public class Vector2Converter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Vector2))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(sourceType); 
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() != typeof(string))
            {
                ErrorManager.ThrowError("Vector2Converter", "Vector2InvalidConversionException");
                return null;
            }
            else
            {
                return ConvertFromString((string)value);
            }
            
        }


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ConvertToString((Vector2)value); 
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            
        }

        public string ConvertToString(Vector2 V2) => $"{V2.X},{V2.Y}";
        public new Vector2 ConvertFromString(string Str) => Vector2.FromString(Str, false);
    }
}