using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utils
{
    class ValidateFieldHelper
    {
        public bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
    }
}
