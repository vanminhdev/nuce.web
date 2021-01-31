using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Helper
{
    public class PropertyCopierOption
    {
        public static int AllowLowerCase = 1;
        public static int NotAllowLowerCase = 2;
    }

    public class PropertyCopier<TSrc, TDes> where TSrc : class
                                            where TDes : class
    {
        public const int AllowLowerCase = 1;
        public const int NotAllowLowerCase = 2;

        public static void Copy(TSrc srcObj, TDes desObj, int option = NotAllowLowerCase)
        {
            var srcProperties = srcObj.GetType().GetProperties();
            var desProperties = desObj.GetType().GetProperties();
            
            foreach (var srcPro in srcProperties)
            {
                foreach (var desPro in desProperties)
                {
                    if (option == AllowLowerCase)
                    {
                        if (srcPro.Name.ToLower() == desPro.Name.ToLower() && srcPro.PropertyType == desPro.PropertyType)
                        {
                            desPro.SetValue(desObj, srcPro.GetValue(srcObj));
                            break;
                        }
                    }
                    else if (option == NotAllowLowerCase)
                    {
                        if (srcPro.Name == desPro.Name && srcPro.PropertyType == desPro.PropertyType)
                        {
                            desPro.SetValue(desObj, srcPro.GetValue(srcObj));
                            break;
                        }
                    }
                }
            }
        }

        public static void Copy(TSrc srcObj, TDes desObj, int option = NotAllowLowerCase, params string[] IgnorePros)
        {
            var srcProperties = srcObj.GetType().GetProperties();
            var desProperties = desObj.GetType().GetProperties();

            foreach (var srcPro in srcProperties.Where(o => !IgnorePros.Contains(o.Name))) //bỏ qua một số property
            {
                foreach (var desPro in desProperties)
                {
                    if (option == AllowLowerCase)
                    {
                        if (srcPro.Name.ToLower() == desPro.Name.ToLower() && srcPro.PropertyType == desPro.PropertyType)
                        {
                            desPro.SetValue(desObj, srcPro.GetValue(srcObj));
                            break;
                        }
                    }
                    else if (option == NotAllowLowerCase)
                    {
                        if (srcPro.Name == desPro.Name && srcPro.PropertyType == desPro.PropertyType)
                        {
                            desPro.SetValue(desObj, srcPro.GetValue(srcObj));
                            break;
                        }
                    }
                }
            }
        }
    }
}
