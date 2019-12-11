using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CherkashinProject
{
    public class CheckForInteger
    {
        public static void CheckForInt(object sender)
        {
            string buf = "";
            char[] array = (sender as TextBox).Text.ToCharArray();
            foreach (var item in array)
            {
                if (Char.IsDigit(item))
                    buf += item;
            }
            (sender as TextBox).Text = buf;
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }
    }
}
