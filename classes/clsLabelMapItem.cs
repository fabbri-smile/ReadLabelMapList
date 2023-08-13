using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLabelMapList
{
    /// <summary>
    /// label_map.txtの項目×1個分の情報を持つクラス
    /// </summary>
    /// <remarks>
    /// 
    /// 【パースする文字列の例】
    /// 
    ///     item {
    ///        name: "/m/01g317"
    ///        id: 1
    ///        display_name: "person"
    ///     }
    /// 
    /// </remarks>

    internal class clsLabelMapItem : clsBaseParser
    {
        public int id = 0;                  // id
        public string display_name = "";    // display_name

#if false   // パターン①
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id_init"></param>
        /// <param name="display_name_init"></param>
        private clsLabelMapItem(int id_init, string display_name_init)
        {
            try
            {
                id = id_init;
                display_name = display_name_init;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // name 行のパーサー (ex : "name: "/m/01g317"" )
        private static readonly Parser<string> nameParser = from _ in Parse.String("name").Token()      // name
                                                            from separator in Parse.Char(':').Token()   // :
                                                            from name in EnclosedStrParser.Token()      // "/m/01g317"
                                                            select name;

        // id 行のパーサー (ex : "id: 1" )
        private static readonly Parser<int> idParser = from _ in Parse.String("id").Token()         // id
                                                       from separator in Parse.Char(':').Token()    // :
                                                       from id in intParser.Token()                 // 1
                                                       select id;

        // display_name 行のパーサー (ex : "display_name: "person"")
        private static readonly Parser<string> display_nameParser = from _ in Parse.String("display_name").Token()  // display_name
                                                                    from separator in Parse.Char(':').Token()       // :
                                                                    from display_name in EnclosedStrParser.Token()  // "person"
                                                                    select display_name;

        // label_map (×1項目) のパーサー
        public static readonly Parser<clsLabelMapItem> LabelMapItemParser = from _ in Parse.String("item").Token()          // item
                                                                            from open in Parse.Char('{').Token()            // {
                                                                            from name in nameParser.Token()                 //    name: "/m/01g317"
                                                                            from id in idParser.Token()                     //    id: 1
                                                                            from display_name in display_nameParser.Token() //    display_name: "person"
                                                                            from close in Parse.Char('}').Token()           // }
                                                                            select new clsLabelMapItem(id, display_name);
#else
#if true    // パターン②
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="KeyValueList"></param>
        private clsLabelMapItem(IEnumerable<KeyValuePair<string, string>> KeyValueList)
        {
            try
            {
                foreach (KeyValuePair<string, string> KeyValue in KeyValueList)
                {
                    switch (KeyValue.Key)
                    {
                        case "id": id = int.Parse(KeyValue.Value); break;
                        case "display_name": display_name = KeyValue.Value; break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 「キー」と「値」のペアに分解するパーサー
        /// 
        ///     【パースする文字列の例】
        ///         例(1) : "name: "/m/01g317""
        ///         例(2) : "id: 1"
        ///         例(3) : "display_name: "person""
        ///
        /// </summary>
        private static Parser<KeyValuePair<string, string>> keyValueParser = from key in keyParser.Token()              // display_name
                                                                             from separator in Parse.Char(':').Token()  // :
                                                                             from value in EnclosedStrParser            // "person"
                                                                                          .Or(strParser).Token()        // 1
                                                                             select new KeyValuePair<string, string>(key, value);
#else       // パターン③
        /// <summary>
        /// 「キー」と「値」のペアを保持するクラス
        /// </summary>
        private class clsKeyValue
        {
            public string Key;
            public string Value;

            public clsKeyValue(string Key_init, string Value_init)
            {
                Key = Key_init;
                Value = Value_init;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="KeyValueList"></param>
        private clsLabelMapItem(IEnumerable<clsKeyValue> KeyValueList)
        {
            try
            {
                foreach (clsKeyValue KeyValue in KeyValueList)
                {
                    switch (KeyValue.Key)
                    {
                        case "id": id = int.Parse(KeyValue.Value); break;
                        case "display_name": display_name = KeyValue.Value; break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 「キー」と「値」のペアに分解するパーサー
        /// 
        ///     【パースする文字列の例】
        ///         例(1) : "name: "/m/01g317""
        ///         例(2) : "id: 1"
        ///         例(3) : "display_name: "person""
        ///
        /// </summary>
        private static readonly Parser<clsKeyValue> keyValueParser = from key in keyParser.Token()               // display_name
                                                                     from separator in Parse.Char(':').Token()   // :
                                                                     from value in EnclosedStrParser             // "person"
                                                                                   .Or(strParser).Token()       // 1
                                                                     select new clsKeyValue(key, value);
#endif

        // label_map (×1項目) のパーサー
        public static readonly Parser<clsLabelMapItem> LabelMapItemParser = from _ in Parse.String("item").Token()     // item
                                                                            from open in Parse.Char('{').Token()       // {
                                                                            from keyValues in keyValueParser.Many()    //    name: "/m/01g317"
                                                                                                                       //    id: 1
                                                                                                                       //    display_name: "person"
                                                                            from close in Parse.Char('}').Token()      // }
                                                                            select new clsLabelMapItem(keyValues);
#endif
    }
}
