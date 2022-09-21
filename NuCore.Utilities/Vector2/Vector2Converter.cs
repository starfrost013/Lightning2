using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace NuCore.Utilities
{
    /// <summary>
    /// There are no string-to-Vector2 converters in the framework,
    /// 
    /// Converts a string to a Vector2.
    /// </summary>
    [TypeConverter(typeof(Vector2))]
    public class Vector2Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string) 
                || base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string initialValue = value.ToString();
                // Microsoft tostring uses < to indicate the start and > to indicate the end so let's strip those out to support this
                initialValue = initialValue.Replace("<", "");
                initialValue = initialValue.Replace(">", "");

                string[] commaSeparatedValues = initialValue.Split(',');

                if (commaSeparatedValues.Length != 2) _ = new NCException("Attempted to convert a non-Vector2 to a Vector2!", 151, 
                    "Vector2Converter::ConvertFrom was not a valid Vector2 - too many or not enough comma separated values",
                    NCExceptionSeverity.FatalError);

                float x = -1, y = -1;

                if (!float.TryParse(commaSeparatedValues[0], out x)
                    || !float.TryParse(commaSeparatedValues[1], out y)) _ = new NCException("Attempted to convert a non-Vector2 to a Vector2!", 152, 
                        "Vector2Converter::ConvertFrom was not a valid Vector2 - one of the comma-separated values was not a float!",
                    NCExceptionSeverity.FatalError);

                return new Vector2(x, y);

            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}