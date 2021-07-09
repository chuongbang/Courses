using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace Course.Web.Components
{
    public partial class LabelFor : ComponentBase
    {
        protected string Name { get; set; }
        protected string Display { get; set; }
        protected bool Required { get; set; }
        [Parameter] public FieldIdentifier FieldIdentifier { get; set; }

        [Parameter] public Expression<Func<string>> For { get; set; }

        protected override void OnParametersSet()
        {
            if (For == null)
            {
                if (FieldIdentifier.Model == null)
                    throw new InvalidOperationException("For or FieldIdentity is required.");
            }
            else
            {
                FieldIdentifier = FieldIdentifier.Create(For);
            }

            Name = FieldIdentifier.FieldName;
            var property = FieldIdentifier.Model.GetType().GetProperty(Name);
            if (property != null)
            {
                var displayAttribute = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false)?.FirstOrDefault();
                if (displayAttribute != null)
                {
                    Display = displayAttribute.Name;
                }
                else
                {
                    Display = Name;
                }
            }
            else
            {
                throw new InvalidOperationException("For is used for property.");
            }

            if (property != null)
            {
                Required = property.GetCustomAttributes(typeof(RequiredAttribute), false).Any();
            }
            else
            {
                throw new InvalidOperationException("For is used for property.");
            }
        }
    }
}
