﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Classes.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum genericEnum)
        {
            Type genericEnumType = genericEnum.GetType();
            System.Reflection.MemberInfo[] memberInfo =
                        genericEnumType.GetMember(genericEnum.ToString());

            if ((memberInfo != null && memberInfo.Length > 0))
            {

                var _Attribs = memberInfo[0].GetCustomAttributes
                      (typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Length > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs[0]).Description;
                }
            }

            return genericEnum.ToString();
        }
    }
}
