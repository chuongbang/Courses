using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Core.Attributes.Web
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HiddenAttribute : Attribute
    {
    }
}
