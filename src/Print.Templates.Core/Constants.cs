using System.Collections.Generic;
using System.Text.RegularExpressions;
using Print.Templates.Core.Enum;

namespace Print.Templates.Core
{
    public static class Constants
    {
        static Constants()
        {
            Alignments = new Dictionary<string, Alignment>
            {
                {"靠左",Alignment.Left},
                {"居中",Alignment.Center},
                {"靠右",Alignment.Right},
            };
            FontSizes = new Dictionary<string, FontSize>
            {
                {"小号",FontSize.Small},
                {"中号",FontSize.Medium},
                {"大号",FontSize.Large}
            };
        }

        public static Dictionary<string,Alignment> Alignments { get; }
        public static Dictionary<string,FontSize> FontSizes { get; }

        /// <summary>
        /// <para>validate width string</para>
        /// <example> * 12 12pt 12px 12* </example>
        /// </summary>
        public static bool ValidateWidth(string width)
        {
            return Regex.IsMatch(width, @"((^[0-9]*\*{0,1}$)|(^[0-9]+(pt{0,1}|px{0,1})$)){1}");
        }
    }
}
