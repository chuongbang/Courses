using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Share.Ultils
{
    public static class DictionaryExtentions
    {
        public static void Add(this Dictionary<string, List<string>> dic, string key, params string[] content)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].AddRange(content);
            }
            else
            {
                dic.Add(key, content.ToList());
            }
        }
    }
}
