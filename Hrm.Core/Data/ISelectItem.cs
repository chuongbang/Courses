using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Core.Data
{
    public interface ISelectItem
    {
        string GetKey();
        string GetDisplay();
        string GetCustomDisplay();
        void SetKey(string key);
    }
}
