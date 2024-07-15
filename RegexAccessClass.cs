using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doviz_Ofisi
{
    internal class RegexAccessClass
    {
        public bool CheckTb(TextBox tb)
        {
            Regex r1 = new Regex(@"^[0-9.,]+$");
            Match m1 = r1.Match(tb.Text);
            if (m1.Success)
                return true;
            else
                return false;
        }

        public bool emailCheck(TextBox tb)
        {
            Regex regEmail = new Regex(@"^[a-zA-Z0-9]+@{1}(.{1}[a-zA-Z0-9]+){1,4}$");
            Match m1 = regEmail.Match(tb.Text);
            if (m1.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool usernameCheck(TextBox tb)
        {
            Regex regUser = new Regex(@"^[a-z A-Z 0-9 _ -]{3,16}$");
            Match m1 = regUser.Match(tb.Text);
            if (m1.Success)
                return true;
            else
                return false;

        }

        public bool pinCheck(TextBox tb)
        {
            Regex regPin = new Regex(@"^[0-9]{4}$");
            Match m1 = regPin.Match(tb.Text);
            if (m1.Success)
                return true;
            else
                return false;
        }

        // ^[A-ZÜĞŞİÖÇ0-9ı_*!@$+%&#^(')-]['A-ZÜĞŞİÖÇ0-9 a-zı?-_*!()şği@$+%&#^üö-]+$
        public bool bookCheck(TextBox tb)
        {
            Regex regBook = new Regex(@"^[A-ZÜĞŞİÖÇ0-9ı_*!@$+%&#^(')-]['A-ZÜĞŞİÖÇ0-9 a-zı?-_*!()şği@$+%&#^üö-]+$");
            Match m1 = regBook.Match(tb.Text);
            if (m1.Success)
                return true;
            else
                return false;
        }
    }
}
