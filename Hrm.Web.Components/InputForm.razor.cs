using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AntDesign;
using Course.Core.Attributes.Web;
using Course.Core.Data;
using Course.Core.Extentions;
using Course.Core.Ultis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Course.Web.Share.Models.EditModels;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using Microsoft.Extensions.Hosting;
using Course.Web.Share.Ultils;
using Course.Core.Enums;
using Blazored.TextEditor;

namespace Course.Web.Components
{
    public partial class InputForm<ItemType> : ComponentBase
    {
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] MessageService Message { get; set; }
        [CascadingParameter(Name = "UploadApiPath")] public string UploadApiPath { get; set; }
        [Parameter] public string Title { get; set; }
        private ItemType _value;

        [Parameter]
        public ItemType Value
        {
            get { return _value; }
            set
            {
                if (ObjectExtentions.EqualValues(value, _value))
                {
                    return;
                }

                _value = value;
            }
        }

        [Parameter] public EventCallback<ItemType> ValueChanged { get; set; }
        [Parameter] public EventCallback CancelChanged { get; set; }
        [Parameter] public bool AllowChangeStyle { get; set; } = true;
        [Parameter] public bool ShowValidate { get; set; }
        [Parameter] public bool ShowActionBar { get; set; } = true;
        [Parameter] public string NamePage { get; set; }

        [Parameter] public string NameLabelSubmit { get; set; } = "Lưu";
        [Parameter] public EventCallback OnValid { get; set; }
        [Parameter] public EventCallback<string> ButtonClick { get; set; }

        protected Action InitForm { get; set; }
        protected Func<string, Dictionary<string, List<string>>> Validate { get; set; }
        protected Func<Dictionary<string, List<string>>> ValidateAll { get; set; }
        protected Func<string, string> GetCustomClassForField { get; set; }
        protected List<PropertyInfo> InputFields { get; set; }
        protected HashSet<string> DisableInputFields { get; set; }
        protected bool ReadOnly { get; set; }
        protected Dictionary<string, Dictionary<string, ISelectItem>> DataSource { get; set; }

        protected bool tempBool;
        Property<EditBaseModel> _property = new Property<EditBaseModel>();
        [Parameter] public int Style { get; set; } = 2;
        string pattern = "form-style-";
        protected string formStyle;
        InputWatcher _inputWatcher;
        bool _requiredRefresh = false;
        bool dialogTamTinh = false;
        string message;

        BlazoredTextEditor _quillHtml;
        [Parameter] public string QuillHtmlContent { get; set; }

        [Parameter] public EventCallback<string> QuillHtmlContentChanged { get; set; }


        protected override void OnParametersSet()
        {
            if (Value == null)
            {
                throw new InvalidOperationException($"Value is null. The type {Value.GetType().FullName} " +
                        $"must have Value property and Value is not null");
            }

            var inputFieldsProperty = Value.GetType().GetProperty(_property.Name(c => c.InputFields));
            if (inputFieldsProperty == null)
            {
                throw new InvalidOperationException($"InputFields is null. The type {Value.GetType().FullName} " +
                    $"must have InputFields property and InputFields is not null");
            }
            InputFields = (List<PropertyInfo>)inputFieldsProperty.GetValue(Value);

            var disableInputFieldsProperty = Value.GetType().GetProperty(_property.Name(c => c.DisableInputFields));
            if (disableInputFieldsProperty == null)
            {
                throw new InvalidOperationException($"DisableInputFields is null. The type {Value.GetType().FullName} " +
                    $"must have DisableInputFields property and DisableInputFields is not null");
            }
            DisableInputFields = (HashSet<string>)disableInputFieldsProperty.GetValue(Value);

            var readOnlyProperty = Value.GetType().GetProperty(_property.Name(c => c.ReadOnly));
            if (readOnlyProperty == null)
            {
                throw new InvalidOperationException($"ReadOnlyProperty is null. The type {Value.GetType().FullName} " +
                    $"must have ReadOnlyProperty property and ReadOnlyProperty is not null");
            }
            ReadOnly = (bool)readOnlyProperty.GetValue(Value);

            var dataSourceProperty = Value.GetType().GetProperty(_property.Name(c => c.DataSource));
            if (dataSourceProperty == null)
            {
                throw new InvalidOperationException($"DataSource is null. The type {Value.GetType().FullName} " +
                    $"must have DataSource property and DataSource is not null");
            }
            DataSource = (Dictionary<string, Dictionary<string, ISelectItem>>)dataSourceProperty.GetValue(Value);
            var nameMethodInitForm = _property.ActionName(c => c.InitForm);
            var nameMethodValidate = _property.FuncName<string, Dictionary<string, List<string>>>(c => c.Validate);
            var nameMethodValidateAll = _property.FuncName<bool, Dictionary<string, List<string>>>(c => c.ValidateAll);
            var nameMethodGetCustomClassForField = _property.FuncName<string, string>(c => c.GetCustomClassForField);
            InitForm = new Action(() => Value.GetType().GetMethod(nameMethodInitForm)?.Invoke(Value, null));
            Validate = new Func<string, Dictionary<string, List<string>>>((nameProperty) => Value.GetType().GetMethod(nameMethodValidate)?.Invoke(Value, new object[] { nameProperty }) as Dictionary<string, List<string>>);
            ValidateAll = new Func<Dictionary<string, List<string>>>(() => Value.GetType().GetMethod(nameMethodValidateAll)?.Invoke(Value, new object[] { false }) as Dictionary<string, List<string>>);
            GetCustomClassForField = new Func<string, string>((p) => Value.GetType()
                                        .GetMethod(nameMethodGetCustomClassForField)?.Invoke(Value, new object[] { p }) as string);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_requiredRefresh || firstRender)
            {
                _requiredRefresh = false;
                InitForm?.Invoke();
            }
            if (ShowValidate)
            {
                if (_inputWatcher.Validate())
                {
                    var customValidateResult = ValidateAll?.Invoke();
                    if (customValidateResult == null || customValidateResult.Any())
                    {
                        var errorMessageStore = customValidateResult.Where(c => c.Value.Any()).ToDictionary(c => c.Key, v => v.Value);
                        _inputWatcher.NotifyFieldChanged(errorMessageStore.First().Key, errorMessageStore);
                        _requiredRefresh = true;
                        StateHasChanged();
                    }
                }
                ShowValidate = false;
            }
        }

        protected async Task OnvalidSubmit()
        {
            try
            {
                if (_quillHtml != null)
                {
                    await QuillHtmlContentChanged.InvokeAsync((await _quillHtml.GetHTML()));
                }
                var customValidateResult = ValidateAll?.Invoke();
                if (customValidateResult != null && !customValidateResult.Any())
                {
                    await ValueChanged.InvokeAsync(_value);
                }
                else
                {
                    var errorMessageStore = customValidateResult.Where(c => c.Value.Any()).ToDictionary(c => c.Key, v => v.Value);
                    _inputWatcher.NotifyFieldChanged(errorMessageStore.First().Key, errorMessageStore);
                    _requiredRefresh = true;
                    StateHasChanged();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void CancelClick()
        {
            CancelChanged.InvokeAsync(null);
        }

        protected override void OnInitialized()
        {
            formStyle = pattern + Style;

        }

        protected void ChangeStyle(int i)
        {
            Style = i;
            formStyle = pattern + Style;
        }

        protected void OnInValidSubmit()
        {
            message = "Dữ liệu không hợp lệ";
        }

        void FieldChanged(PropertyInfo property, object value, bool requiredRefresh = false)
        {
            // property.SetValue(Value, value);
            Value.SetValue(property.Name, value);
            NotifyFieldChanged(property.Name, Validate.Invoke(property.Name));

        }

        public void NotifyFieldChanged(string propertyName, Dictionary<string, List<string>> validateMessages)
        {
            _inputWatcher.NotifyFieldChanged(propertyName, validateMessages);
            _requiredRefresh = (Value.GetValue(_property.Name(c => c.RequiredRefresh)) as bool? == true);
            StateHasChanged();
        }

        public void NotifyFieldChanged(List<string> propertyNames)
        {
            foreach (var name in propertyNames)
            {
                _inputWatcher.NotifyFieldChanged(name, Validate.Invoke(name));
            }
            _requiredRefresh = (Value.GetValue(_property.Name(c => c.RequiredRefresh)) as bool? == true);
            StateHasChanged();
        }

        public void RerenderForm()
        {
            // _requiredRefresh = true;
            InitForm.Invoke();
            StateHasChanged();
        }

        public void Submit()
        {
            if (_inputWatcher.Validate())
            {
                var customValidateResult = ValidateAll?.Invoke();
                if (customValidateResult != null && !customValidateResult.Any())
                {
                    OnValid.InvokeAsync(null);
                }
                else
                {
                    var errorMessageStore = customValidateResult.Where(c => c.Value.Any()).ToDictionary(c => c.Key, v => v.Value);
                    _inputWatcher.NotifyFieldChanged(errorMessageStore.First().Key, errorMessageStore);
                    _requiredRefresh = true;
                    StateHasChanged();
                }
            }
        }

        public static DateTime? ConvertStringToDateTime(string input)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            CultureInfo culture1 = CultureInfo.CreateSpecificCulture("vi-VN");
            DateTime dateTime;
            if (input == null || input == string.Empty)
            {
                return null;
            }
            if (System.DateTime.TryParseExact(input, "dd/MM/yyyy", culture1, DateTimeStyles.None, out dateTime) && dateTime != new DateTime())
            {
                return dateTime;
            }
            if (System.DateTime.TryParseExact(input, "MM/yyyy", culture1, DateTimeStyles.None, out dateTime) && dateTime != new DateTime())
            {
                return dateTime;
            }
            if (System.DateTime.TryParseExact(input, "yyyy", culture1, DateTimeStyles.None, out dateTime) && dateTime != new DateTime())
            {
                return dateTime;
            }
            if (culture.Equals(culture1))
            {
                System.DateTime.TryParseExact(input, "dd/MM/yyyy", culture1, DateTimeStyles.None, out dateTime);
            }
            else
            {
                System.DateTime.TryParseExact(input, "dd/MM/yyyy", culture, DateTimeStyles.AssumeLocal, out dateTime);
                if (dateTime == new DateTime())
                    System.DateTime.TryParseExact(input, "dd/MM/yyyy", culture1, DateTimeStyles.None, out dateTime);
            }

            if (dateTime == DateTime.MinValue)
            {
                return null;
            }
            return dateTime;
        }


        private void OnButtonClick(string propertyName)
        {
            ButtonClick.InvokeAsync(propertyName);
        }

        Dictionary<string, bool> loadings = new Dictionary<string, bool>();

        string imageUrl;

        bool BeforeUpload(UploadFileItem file, FileAttribute fileOptions)
        {
            var isAllowType = fileOptions?.AllowType == null || fileOptions?.AllowType?.Contains(file.Type) == true;
            if (!isAllowType)
            {
                Message.Error("Bạn chỉ được phép tải lên JPG/PNG file!");
            }
            var isLessThanMaxSize = file.Size / 1024 / 1024 < fileOptions.MaxSize;
            if (!isLessThanMaxSize)
            {
                Message.Error($"Ảnh phải có kính thước dưới {fileOptions.MaxSize / 1024 / 1024}MB!");
            }
            return isAllowType && isLessThanMaxSize;
        }

        void UploadSingleFileChange(UploadInfo fileinfo, PropertyInfo property)
        {
            loadings[property.Name] = fileinfo.File.State == UploadState.Uploading;

            if (fileinfo.File.State == UploadState.Success)
            {
                property.SetValue(Value, fileinfo.File.ObjectURL);
            }
            InvokeAsync(StateHasChanged);
        }

        List<IBrowserFile> loadedFiles = new();
        long maxFileSize = 1024 * 1024 * 100;
        int maxAllowedFiles = 1;
        string NameFile;
        int progressPercent;
        bool displayProgress = false;
        private async Task LoadFiles(InputFileChangeEventArgs e, PropertyInfo property)
        {
            loadedFiles.Clear();
            NameFile = string.Empty;
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    loadedFiles.Add(file);
                    NameFile += file.Name;
                    var trustedFileNameForFileStorage = file.Name;
                    var pathFolder = Path.Combine(GlobalVariants.FileUploadPath);
                    var pathFile = Path.Combine(pathFolder, trustedFileNameForFileStorage);
                    if (!Directory.Exists(Path.GetDirectoryName(pathFolder)))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }
                    using var fileUpload = File.OpenWrite(pathFile);
                    using var stream = file.OpenReadStream(968435456);

                    var buffer = new byte[4 * 1096];
                    int bytesRead;
                    double totalRead = 0;
                    var cancelation = new CancellationTokenSource();
                    displayProgress = true;

                    while ((bytesRead = await stream.ReadAsync(buffer, cancelation.Token)) != 0)
                    {
                        totalRead += bytesRead;
                        await fileUpload.WriteAsync(buffer, cancelation.Token);

                        progressPercent = (int)((totalRead / file.Size) * 100);
                        StateHasChanged();
                    }

                    displayProgress = false;

                    //await using FileStream fs = new(pathFile, FileMode.Create);
                    //await file.OpenReadStream(maxFileSize).CopyToAsync(fs);


                }
                catch (Exception ex)
                {
                }
            }

            FieldChanged(property, NameFile);

        }





    }
}
