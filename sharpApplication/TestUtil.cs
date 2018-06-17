using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sharpApplication
{
    public class FatherCls
    {
        public void work()
        {
            System.Console.WriteLine("Busy");
        }
    }

    public class SonClsA: FatherCls
    { }

    public class SonClsB : FatherCls
    { }

    public class TestUtil
    {
        public static bool IsNumberByReg(string str)
        {
            if (str == null)
            {
                return false;
            }
            return Regex.IsMatch(str, @"^/d+$", RegexOptions.Singleline);
        }

        public static bool IsNumberByChar(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str, i))
                    return false;
            }
            return true;
        }

        public static bool IsNumberByTryParse(String str)
        {
            string s = str;
            int i = 0;
            bool result = int.TryParse(s, out i);
            if (result)
            {
                Console.WriteLine("It is a number string !");
            }
            else
            {
                Console.WriteLine("It is not a number string !");
            }

            return result;
        }
    }
}
