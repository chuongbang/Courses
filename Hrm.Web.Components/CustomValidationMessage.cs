using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace Course.Web.Components
{
    public class CustomValidationMessage<TValue> : ComponentBase, IDisposable
    {
        private EditContext _previousEditContext;
        private Expression<Func<TValue>> _previousFieldAccessor;
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private FieldIdentifier _fieldIdentifier;

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created <c>div</c> element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [CascadingParameter] EditContext CurrentEditContext { get; set; }

        ///// <summary>
        ///// Specifies the field for which validation messages should be displayed.
        ///// </summary>
        //[Parameter] public Expression<Func<TValue>> For { get; set; }


        /// <summary>
        /// Specifies the Field for which validation messages should be displayed.
        /// </summary>
        [Parameter] public FieldIdentifier Field { get; set; }

        /// <summary>`
        /// Constructs an instance of <see cref="ValidationMessage{TValue}"/>.
        /// </summary>
        public CustomValidationMessage()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                    $"an {nameof(EditForm)}.");
            }

            //if (For == null) // Not possible except if you manually specify T
            //{
            //    throw new InvalidOperationException($"{GetType()} requires a value for the " +
            //        $"{nameof(For)} parameter.");
            //}
            //else if (For != _previousFieldAccessor)
            //{
            //    _fieldIdentifier = FieldIdentifier.Create(For);
            //    _previousFieldAccessor = For;
            //}
            if (!_fieldIdentifier.Equals(Field))
            {
                _fieldIdentifier = Field;
            }


            if (CurrentEditContext != _previousEditContext)
            {
                DetachValidationStateChangedListener();
                CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
                _previousEditContext = CurrentEditContext;
            }
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            foreach (var message in CurrentEditContext.GetValidationMessages(_fieldIdentifier))
            {
                builder.OpenElement(0, "div");
                builder.AddMultipleAttributes(1, AdditionalAttributes);
                builder.AddAttribute(2, "class", "validation-message");
                builder.AddContent(3, message);
                builder.CloseElement();
            }
        }

        private void HandleValidationStateChanged(object sender, ValidationStateChangedEventArgs eventArgs)
        {
            StateHasChanged();
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            DetachValidationStateChangedListener();
            Dispose(disposing: true);
        }

        private void DetachValidationStateChangedListener()
        {
            if (_previousEditContext != null)
            {
                _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
        }
    }
}
