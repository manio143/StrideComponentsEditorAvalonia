using System;
using System.Text;

namespace Stride.Editor.Design.Core.StringUtils
{
    public static class TypeNameToString
    {
        public static string CamelcaseToSpaces(this string name)
        {
            var nameBuilder = new StringBuilder();
            for (var i = 0; i < name.Length - 1; i++)
            {
                nameBuilder.Append(name[i]);
                if (Char.IsLower(name[i]) && Char.IsUpper(name[i + 1]))
                    nameBuilder.Append(' ');
            }
            nameBuilder.Append(name[name.Length - 1]);
            return nameBuilder.ToString();
        }
    }
}
