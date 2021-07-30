using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Course.Web.Components
{
    public partial class RichInputText: ComponentBase
    {
        [Parameter]
        public RenderFragment EditorContent { get; set; }

        [Parameter]
        public RenderFragment ToolbarContent { get; set; }

        [Parameter]
        public bool ReadOnly { get; set; }
            = false;

        [Parameter]
        public string Placeholder { get; set; }
            = "Compose an epic...";

        [Parameter]
        public string Theme { get; set; }
            = "snow";

        [Parameter]
        public string DebugLevel { get; set; }
            = "info";

        [Parameter] public string HtmlContent { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public EventCallback<InputFileChangeEventArgs> InsertImageChange { get; set; }

        private ElementReference QuillElement;
        private ElementReference ToolBar;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RichInputTextInterop.CreateQuill(
                    JSRuntime,
                    QuillElement,
                    ToolBar,
                    ReadOnly,
                    Placeholder,
                    Theme,
                    DebugLevel);

                if (HtmlContent != null)
                {
                    await LoadHTMLContent(HtmlContent);
                }
            }
        }

        public async Task<string> GetText()
        {
            return await RichInputTextInterop.GetText(
                JSRuntime, QuillElement);
        }

        public async Task<string> GetHTML()
        {
            return await RichInputTextInterop.GetHTML(
                JSRuntime, QuillElement);
        }

        public async Task<string> GetContent()
        {
            return await RichInputTextInterop.GetContent(
                JSRuntime, QuillElement);
        }

        public async Task LoadContent(string Content)
        {
            var QuillDelta =
                await RichInputTextInterop.LoadQuillContent(
                    JSRuntime, QuillElement, Content);
        }

        public async Task LoadHTMLContent(string quillHTMLContent)
        {
            var QuillDelta =
                await RichInputTextInterop.LoadQuillHTMLContent(
                    JSRuntime, QuillElement, quillHTMLContent);
        }

        public async Task InsertImage(string ImageURL)
        {
            var QuillDelta =
                await RichInputTextInterop.InsertQuillImage(
                    JSRuntime, QuillElement, ImageURL);
        }
        public async Task InsertImages(List<string> ImageURLs)
        {
            await RichInputTextInterop.InsertQuillImages(
                JSRuntime, QuillElement, ImageURLs);
        }

        public async Task EnableEditor(bool mode)
        {
            var QuillDelta =
                await RichInputTextInterop.EnableQuillEditor(
                    JSRuntime, QuillElement, mode);
        }
    }
}
