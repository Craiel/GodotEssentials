namespace Craiel.Essentials.GameData;

using System;
using System.ComponentModel;
using System.Globalization;
using Utils;

public class GameDataIdTypeConverter : TypeConverter
{
    private const char Separator = ':';

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == TypeDef<string>.Value)
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }
    
    // Overrides the ConvertFrom method of TypeConverter.
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string typed)
        {
            return new GameDataId(uint.Parse(typed));
        }

        return base.ConvertFrom(context, culture, value);
    }
    // Overrides the ConvertTo method of TypeConverter.
    
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == TypeDef<string>.Value)
        {
            return ((GameDataId)value).Value;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}