using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Course.Core.Data;

namespace Course.Web.Share.Models.EditModels
{
    public abstract class EditBaseModel
    {
        public virtual Dictionary<string, Dictionary<string, ISelectItem>> DataSource { get; set; } = new Dictionary<string, Dictionary<string, ISelectItem>>();
        public List<PropertyInfo> InputFields { get; set; } = new List<PropertyInfo>();
        public List<string> FieldsChanged { get; set; } = new List<string>();
        public HashSet<string> DisableInputFields { get; set; } = new HashSet<string>();

        public bool RequiredRefresh { get; set; }
        public bool ReadOnly { get; set; }

        int hashCode;
        public virtual Task InitFormAsync()
        {
            return Task.FromResult(default(object));
        }

        public void InitForm()
        {
            InitFormAsync();
        }
        public virtual void LoadViewCombobox()
        {

        }
        public virtual Dictionary<string, List<string>> Validate(string nameProperty)
        {
            return null;
        }

        public virtual Dictionary<string, List<string>> ValidateAll(bool breakIfError = false)
        {
            Dictionary<string, List<string>> validateMessageStore = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> validateMessage;
            foreach (var field in InputFields)
            {
                validateMessage = Validate(field.Name);
                if (validateMessage != null && validateMessage.Any())
                {
                    if (breakIfError)
                    {
                        var message = validateMessage.First();
                        validateMessageStore.Add(message.Key, message.Value);
                        return validateMessageStore;
                    }
                    foreach (var messageStore in validateMessage)
                    {
                        if (validateMessageStore.ContainsKey(messageStore.Key))
                        {
                            continue;
                        }
                        validateMessageStore.Add(messageStore.Key, messageStore.Value);
                    }
                }
            }
            return validateMessageStore;
        }

        public virtual string GetCustomClassForField(string nameProperty)
        {
            return string.Empty;
        }

        public override int GetHashCode()
        {
            if (hashCode == 0)
            {
                hashCode = base.GetHashCode();
            }
            return hashCode;
        }

    }
}
