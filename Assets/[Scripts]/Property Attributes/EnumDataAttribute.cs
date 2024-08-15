using System;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class EnumDataAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public EnumDataAttribute(Type enumType)
        {
            names = Enum.GetNames(enumType);
        }
    }
}
