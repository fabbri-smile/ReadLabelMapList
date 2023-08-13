using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLabelMapList
{
    internal class clsBaseParser
    {
        /// <summary>
        /// 数字が1文字以上連続している部分の文字列を int 変換して返す
        /// </summary>
        protected static readonly Parser<int> intParser = from num_str in Parse.Digit.AtLeastOnce().Text()
                                                          select int.Parse(num_str);

        /// <summary>
        /// 文字、または数字が1文字以上連続している部分の文字列を返す
        /// (カンマなどの区切り文字は含まれない)
        /// </summary>
        protected static readonly Parser<string> strParser = from str in Parse.LetterOrDigit.AtLeastOnce().Text()
                                                             select str;

        /// <summary>
        /// 文字、数字、または '_' が1文字以上連続している部分の文字列を返す
        /// (カンマなどの区切り文字は含まれない)
        /// </summary>
        protected static readonly Parser<string> keyParser = from key in Parse.LetterOrDigit.Or(Parse.Char('_')).AtLeastOnce().Text()
                                                             select key;
        /// <summary>
        /// ダブルクォートで囲まれた文字列の、内部の文字列を返す
        /// (文字列の中にダブルクォーテーションが入ってる場合は死ぬ)
        /// </summary>
        protected static readonly Parser<string> EnclosedStrParser = from open in Parse.Char('"')
                                                                     from str in Parse.CharExcept('"').AtLeastOnce().Text()
                                                                     from close in Parse.Char('"')
                                                                     select str;
    }
}
