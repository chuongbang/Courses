using Course.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Course.Core.Extentions;

namespace Course.Web.Share.Categories
{
    public class CategoryBase<T> where T : ISelectItem
    {
        protected string PathFile;
        private Dictionary<string, T> _datas;
        private HashSet<string> _keys;
        private List<T> _values;
        public Dictionary<string, T> Datas
        {
            get
            {
                if (_datas == null)
                {
                    _datas = new Dictionary<string, T>();
                    try
                    {
                        _datas = Values.Select(c => new { Key = c.GetKey(), Value = c }).ToDictionary(c => c.Key, v => v.Value);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return _datas;
            }
        }

        public List<T> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new List<T>();
                    string jsonData = System.IO.File.ReadAllText(PathFile);
                    _values = System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonData);
                }
                return _values;
            }
        }

        public HashSet<string> GetKeys()
        {
            if (_keys == null)
            {
                _keys = Values.Select(c => c.GetKey()).Distinct().ToHashSet();
            }
            return _keys;
        }

        public string GetValue(string key)
        {
            if (key.IsNotNullOrEmpty())
            {
                if (Datas.ContainsKey(key))
                {
                    return Datas[key].GetDisplay();
                }
            }

            return string.Empty;
        }

        public void SetPathFile(string path)
        {
            if (File.Exists(path))
                PathFile = path;
            else
                throw new FileNotFoundException($"File danh mục {typeof(T).Name} không tồn tại");
        }
    }
}
