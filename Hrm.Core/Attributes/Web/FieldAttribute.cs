using Course.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Core.Attributes.Web
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class FieldAttribute : Attribute
    {
        public FieldType Type { get; set; }
        public bool CustomDisplay { get; set; }
        public FieldAttribute(FieldType type, bool customDisplay = false)
        {
            Type = type;
            CustomDisplay = customDisplay;
        }

        public FieldAttribute()
        {
            Type = FieldType.TextBox;
            CustomDisplay = false;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DigitAttribute : Attribute
    {
        public int IntergerDigits { get; set; }
        public int Digits { get; set; }

        public double Max { get; set; }

        public DigitAttribute(int intergerDigits, int digits, double max = 9999999999999999999.999)
        {
            IntergerDigits = intergerDigits;
            Digits = digits;
            Max = max;
        }

        public DigitAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class FileAttribute : Attribute
    {
        public string AllowType { get; set; }
        public int MaxSize { get; set; } = int.MaxValue;
        public FileAttribute(string allowType, int maxSize)
        {
            AllowType = allowType;
            MaxSize = maxSize;
        }
    }
}
