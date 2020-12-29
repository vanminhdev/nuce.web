using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nuce.web.api.Helper
{
    public class StringHelper
    {
        public static string ConvertToLatin(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        /// <summary>
        /// Trả về chuỗi đã loại bỏ tiền tố
        /// List prefix ngăn cách bằng dấu phẩy
        /// </summary>
        /// <param name="originString"></param>
        /// <param name="prefixStrList"></param>
        /// <returns></returns>
        public static string WithoutPrefix(string originString, string prefixStrList)
        {
            var normalizedOriginString = ConvertToLatin(originString);
            var prefixList = prefixStrList.Split(',');
            foreach (var prefixStr in prefixList)
            {
                if (normalizedOriginString.ToLower().StartsWith(ConvertToLatin(prefixStr).ToLower()))
                {
                    return originString.Substring(prefixStr.Length);
                }
            }
            return originString;
        }
    }
}
