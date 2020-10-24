using System;
using System.Security.Cryptography;
using System.Text;

namespace nuce.web.data
{
    // hashedPassword = PasswordUtil.HashPassword(password) 

    public class PasswordUtil
    {
        private static string[,] aqCode = new string[94, 2];
        private static string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[size];
            rNGCryptoServiceProvider.GetBytes(array);
            return Convert.ToBase64String(array);
        }
        private static string CreatePasswordHash(string pwd, string salt)
        {
            return PasswordUtil.HashPassword(pwd);
        }
        public static string HashPassword(string pwd)
        {
            string result;
            if (string.IsNullOrEmpty(pwd))
            {
                result = "";
            }
            else
            {
                SHA1 sHA = new SHA1Managed();
                byte[] bytes = Encoding.Default.GetBytes(pwd);
                byte[] array = sHA.TransformFinalBlock(bytes, 0, bytes.Length);
                string text = "";
                byte[] hash = sHA.Hash;
                for (int i = 0; i < hash.Length; i++)
                {
                    byte value = hash[i];
                    text += Convert.ToString(value, 16);
                }
                sHA.Clear();
                result = text;
            }
            return result;
        }
        private static void init()
        {
            PasswordUtil.aqCode[0, 0] = "!";
            PasswordUtil.aqCode[0, 1] = "aqvv7KGH";
            PasswordUtil.aqCode[1, 0] = "\"";
            PasswordUtil.aqCode[1, 1] = "aqvdGzNZ";
            PasswordUtil.aqCode[2, 0] = "#";
            PasswordUtil.aqCode[2, 1] = "aqvEZWy2";
            PasswordUtil.aqCode[3, 0] = "$";
            PasswordUtil.aqCode[3, 1] = "aqv4a5nr";
            PasswordUtil.aqCode[4, 0] = "%";
            PasswordUtil.aqCode[4, 1] = "aqvI8ruG";
            PasswordUtil.aqCode[5, 0] = "&";
            PasswordUtil.aqCode[5, 1] = "aqvSkuT2";
            PasswordUtil.aqCode[6, 0] = "'";
            PasswordUtil.aqCode[6, 1] = "aqv8lshh";
            PasswordUtil.aqCode[7, 0] = "(";
            PasswordUtil.aqCode[7, 1] = "aqvwVdGB";
            PasswordUtil.aqCode[8, 0] = ")";
            PasswordUtil.aqCode[8, 1] = "aqvcy1cu";
            PasswordUtil.aqCode[9, 0] = "*";
            PasswordUtil.aqCode[9, 1] = "aqvVWWuP";
            PasswordUtil.aqCode[10, 0] = "+";
            PasswordUtil.aqCode[10, 1] = "aqv6ZNDw";
            PasswordUtil.aqCode[11, 0] = ",";
            PasswordUtil.aqCode[11, 1] = "aqv55hAm";
            PasswordUtil.aqCode[12, 0] = "-";
            PasswordUtil.aqCode[12, 1] = "aqvkcj4A";
            PasswordUtil.aqCode[13, 0] = ".";
            PasswordUtil.aqCode[13, 1] = "aqvGBsIh";
            PasswordUtil.aqCode[14, 0] = "/";
            PasswordUtil.aqCode[14, 1] = "aqvwgR4S";
            PasswordUtil.aqCode[15, 0] = "0";
            PasswordUtil.aqCode[15, 1] = "aqvXAiwW";
            PasswordUtil.aqCode[16, 0] = "1";
            PasswordUtil.aqCode[16, 1] = "aqvAojAf";
            PasswordUtil.aqCode[17, 0] = "2";
            PasswordUtil.aqCode[17, 1] = "aqvHZho7";
            PasswordUtil.aqCode[18, 0] = "3";
            PasswordUtil.aqCode[18, 1] = "aqvLHgw8";
            PasswordUtil.aqCode[19, 0] = "4";
            PasswordUtil.aqCode[19, 1] = "aqve7Hv4";
            PasswordUtil.aqCode[20, 0] = "5";
            PasswordUtil.aqCode[20, 1] = "aqv13dAE";
            PasswordUtil.aqCode[21, 0] = "6";
            PasswordUtil.aqCode[21, 1] = "aqvDlS6C";
            PasswordUtil.aqCode[22, 0] = "7";
            PasswordUtil.aqCode[22, 1] = "aqvXU57q";
            PasswordUtil.aqCode[23, 0] = "8";
            PasswordUtil.aqCode[23, 1] = "aqvSw082";
            PasswordUtil.aqCode[24, 0] = "9";
            PasswordUtil.aqCode[24, 1] = "aqvB8Ce0";
            PasswordUtil.aqCode[25, 0] = ":";
            PasswordUtil.aqCode[25, 1] = "aqvyNetj";
            PasswordUtil.aqCode[26, 0] = ";";
            PasswordUtil.aqCode[26, 1] = "aqv6uvem";
            PasswordUtil.aqCode[27, 0] = "<";
            PasswordUtil.aqCode[27, 1] = "aqvk5U0n";
            PasswordUtil.aqCode[28, 0] = "=";
            PasswordUtil.aqCode[28, 1] = "aqvbkECy";
            PasswordUtil.aqCode[29, 0] = ">";
            PasswordUtil.aqCode[29, 1] = "aqviVVG0";
            PasswordUtil.aqCode[30, 0] = "?";
            PasswordUtil.aqCode[30, 1] = "aqvCtVYq";
            PasswordUtil.aqCode[31, 0] = "@";
            PasswordUtil.aqCode[31, 1] = "aqvrySOQ";
            PasswordUtil.aqCode[32, 0] = "A";
            PasswordUtil.aqCode[32, 1] = "aqvqcnvl";
            PasswordUtil.aqCode[33, 0] = "B";
            PasswordUtil.aqCode[33, 1] = "aqvPrnQm";
            PasswordUtil.aqCode[34, 0] = "C";
            PasswordUtil.aqCode[34, 1] = "aqvr34en";
            PasswordUtil.aqCode[35, 0] = "D";
            PasswordUtil.aqCode[35, 1] = "aqv7a1Lf";
            PasswordUtil.aqCode[36, 0] = "E";
            PasswordUtil.aqCode[36, 1] = "aqv5eDYu";
            PasswordUtil.aqCode[37, 0] = "F";
            PasswordUtil.aqCode[37, 1] = "aqvnm6mv";
            PasswordUtil.aqCode[38, 0] = "G";
            PasswordUtil.aqCode[38, 1] = "aqvrt2bY";
            PasswordUtil.aqCode[39, 0] = "H";
            PasswordUtil.aqCode[39, 1] = "aqvhywMz";
            PasswordUtil.aqCode[40, 0] = "I";
            PasswordUtil.aqCode[40, 1] = "aqvESPpU";
            PasswordUtil.aqCode[41, 0] = "J";
            PasswordUtil.aqCode[41, 1] = "aqvJr629";
            PasswordUtil.aqCode[42, 0] = "K";
            PasswordUtil.aqCode[42, 1] = "aqvfWhgs";
            PasswordUtil.aqCode[43, 0] = "L";
            PasswordUtil.aqCode[43, 1] = "aqvN6GyV";
            PasswordUtil.aqCode[44, 0] = "M";
            PasswordUtil.aqCode[44, 1] = "aqvEOaZP";
            PasswordUtil.aqCode[45, 0] = "N";
            PasswordUtil.aqCode[45, 1] = "aqvLdv1j";
            PasswordUtil.aqCode[46, 0] = "O";
            PasswordUtil.aqCode[46, 1] = "aqv6SQXm";
            PasswordUtil.aqCode[47, 0] = "P";
            PasswordUtil.aqCode[47, 1] = "aqv8wLWz";
            PasswordUtil.aqCode[48, 0] = "Q";
            PasswordUtil.aqCode[48, 1] = "aqv7X17M";
            PasswordUtil.aqCode[49, 0] = "R";
            PasswordUtil.aqCode[49, 1] = "aqvI1x0a";
            PasswordUtil.aqCode[50, 0] = "S";
            PasswordUtil.aqCode[50, 1] = "aqvdBtGH";
            PasswordUtil.aqCode[51, 0] = "T";
            PasswordUtil.aqCode[51, 1] = "aqvCYhY5";
            PasswordUtil.aqCode[52, 0] = "U";
            PasswordUtil.aqCode[52, 1] = "aqvNge4p";
            PasswordUtil.aqCode[53, 0] = "V";
            PasswordUtil.aqCode[53, 1] = "aqvreOve";
            PasswordUtil.aqCode[54, 0] = "W";
            PasswordUtil.aqCode[54, 1] = "aqvWhU1Q";
            PasswordUtil.aqCode[55, 0] = "X";
            PasswordUtil.aqCode[55, 1] = "aqviBcT9";
            PasswordUtil.aqCode[56, 0] = "Y";
            PasswordUtil.aqCode[56, 1] = "aqvsUIXK";
            PasswordUtil.aqCode[57, 0] = "Z";
            PasswordUtil.aqCode[57, 1] = "aqvengXT";
            PasswordUtil.aqCode[58, 0] = "[";
            PasswordUtil.aqCode[58, 1] = "aqvAeDGc";
            PasswordUtil.aqCode[59, 0] = "\\";
            PasswordUtil.aqCode[59, 1] = "aqvrQzB8";
            PasswordUtil.aqCode[60, 0] = "]";
            PasswordUtil.aqCode[60, 1] = "aqv4AUGU";
            PasswordUtil.aqCode[61, 0] = "^";
            PasswordUtil.aqCode[61, 1] = "aqvB3PMS";
            PasswordUtil.aqCode[62, 0] = "_";
            PasswordUtil.aqCode[62, 1] = "aqvpKx7c";
            PasswordUtil.aqCode[63, 0] = "`";
            PasswordUtil.aqCode[63, 1] = "aqvXnWFy";
            PasswordUtil.aqCode[64, 0] = "a";
            PasswordUtil.aqCode[64, 1] = "aqvdgrHA";
            PasswordUtil.aqCode[65, 0] = "b";
            PasswordUtil.aqCode[65, 1] = "aqv3eg4q";
            PasswordUtil.aqCode[66, 0] = "c";
            PasswordUtil.aqCode[66, 1] = "aqvQijOG";
            PasswordUtil.aqCode[67, 0] = "d";
            PasswordUtil.aqCode[67, 1] = "aqvAbdas";
            PasswordUtil.aqCode[68, 0] = "e";
            PasswordUtil.aqCode[68, 1] = "aqvO3afC";
            PasswordUtil.aqCode[69, 0] = "f";
            PasswordUtil.aqCode[69, 1] = "aqvbXhJD";
            PasswordUtil.aqCode[70, 0] = "g";
            PasswordUtil.aqCode[70, 1] = "aqvaCOzN";
            PasswordUtil.aqCode[71, 0] = "h";
            PasswordUtil.aqCode[71, 1] = "aqvYweCg";
            PasswordUtil.aqCode[72, 0] = "i";
            PasswordUtil.aqCode[72, 1] = "aqvfuuCB";
            PasswordUtil.aqCode[73, 0] = "j";
            PasswordUtil.aqCode[73, 1] = "aqvmmvKy";
            PasswordUtil.aqCode[74, 0] = "k";
            PasswordUtil.aqCode[74, 1] = "aqvfF6hO";
            PasswordUtil.aqCode[75, 0] = "l";
            PasswordUtil.aqCode[75, 1] = "aqvygQUl";
            PasswordUtil.aqCode[76, 0] = "m";
            PasswordUtil.aqCode[76, 1] = "aqvaWs7Q";
            PasswordUtil.aqCode[77, 0] = "n";
            PasswordUtil.aqCode[77, 1] = "aqvZ0PiH";
            PasswordUtil.aqCode[78, 0] = "o";
            PasswordUtil.aqCode[78, 1] = "aqvJfEwE";
            PasswordUtil.aqCode[79, 0] = "p";
            PasswordUtil.aqCode[79, 1] = "aqv8EuYf";
            PasswordUtil.aqCode[80, 0] = "q";
            PasswordUtil.aqCode[80, 1] = "aqvVgJyr";
            PasswordUtil.aqCode[81, 0] = "r";
            PasswordUtil.aqCode[81, 1] = "aqvtZjX0";
            PasswordUtil.aqCode[82, 0] = "s";
            PasswordUtil.aqCode[82, 1] = "aqvaHIWj";
            PasswordUtil.aqCode[83, 0] = "t";
            PasswordUtil.aqCode[83, 1] = "aqvKeAYU";
            PasswordUtil.aqCode[84, 0] = "u";
            PasswordUtil.aqCode[84, 1] = "aqv1wjyP";
            PasswordUtil.aqCode[85, 0] = "v";
            PasswordUtil.aqCode[85, 1] = "aqv3wAOJ";
            PasswordUtil.aqCode[86, 0] = "w";
            PasswordUtil.aqCode[86, 1] = "aqvm5XwY";
            PasswordUtil.aqCode[87, 0] = "x";
            PasswordUtil.aqCode[87, 1] = "aqvTexe2";
            PasswordUtil.aqCode[88, 0] = "y";
            PasswordUtil.aqCode[88, 1] = "aqvOXlRi";
            PasswordUtil.aqCode[89, 0] = "z";
            PasswordUtil.aqCode[89, 1] = "aqv0uldu";
            PasswordUtil.aqCode[90, 0] = "{";
            PasswordUtil.aqCode[90, 1] = "aqvSORA6";
            PasswordUtil.aqCode[91, 0] = "|";
            PasswordUtil.aqCode[91, 1] = "aqvQW83A";
            PasswordUtil.aqCode[92, 0] = "}";
            PasswordUtil.aqCode[92, 1] = "aqvZDdlt";
            PasswordUtil.aqCode[93, 0] = "~";
            PasswordUtil.aqCode[93, 1] = "aqvXQxaM";
        }
        public static string AQEncoding(string t)
        {
            string text = "";
            PasswordUtil.init();
            for (int i = 0; i < t.Length; i++)
            {
                string text2 = "";
                for (int j = 0; j <= 93; j++)
                {
                    text2 += t[i];
                    if (PasswordUtil.aqCode[j, 0].Equals(text2))
                    {
                        text += PasswordUtil.aqCode[j, 1];
                    }
                    text2 = "";
                }
            }
            return text;
        }
        public static string AQDecoding(string chuoi)
        {
            string text = "";
            PasswordUtil.init();
            string result;
            for (int i = 0; i < chuoi.Length; i += 8)
            {
                string text2 = "";
                if (chuoi.Length % 8 != 0)
                {
                    result = "";
                    return result;
                }
                for (int j = i; j < i + 8; j++)
                {
                    text2 += chuoi[j];
                }
                for (int k = 0; k <= 93; k++)
                {
                    if (text2.Equals(PasswordUtil.aqCode[k, 1]))
                    {
                        text += PasswordUtil.aqCode[k, 0];
                    }
                }
            }
            result = text;
            return result;
        }

        //public static string getPass(string mand)
        //{
        //    DataClassesDataContext dc = new DataClassesDataContext();
        //    var query = (from x in dc.NguoiDungs
        //                 where x.MaND.Equals(mand)
        //                 select new { x.MatKhau }).Single();
        //    string mk = query.MatKhau;
        //    return (HashPassword("admin2341"));
        //}

    }
}