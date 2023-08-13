using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLabelMapList
{
    /// <summary>
    /// label_map.txtの中に入っている全ての項目の情報を持つクラス
    /// </summary>
    /// <remarks>
    /// 
    ///     item {
    ///        name: "/m/01g317"
    ///        id: 1
    ///        display_name: "person"
    ///     }
    ///     item {
    ///        name: "/m/0199g"
    ///        id: 2
    ///        display_name: "bicycle"
    ///     }
    ///             ・
    ///             ・続く
    ///             ・
    /// 
    /// </remarks>
    internal class clsLabelMapList
    {
        // label_map.txtから読込んだデータのリスト
        private List<clsLabelMapItem> prv_LabelMapList = null;

        /// <summary>
        /// プロパティ定義 : label_map.txtから読込んだデータのリスト
        /// </summary>
        public List<clsLabelMapItem> LabelMapList
        {
            get { return prv_LabelMapList; }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public clsLabelMapList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LabelMapList_init">label_map.txtから読込んだデータのリスト</param>
        private clsLabelMapList(IEnumerable<clsLabelMapItem> LabelMapList_init)
        {
            prv_LabelMapList = LabelMapList_init.ToList();
        }

        /// <summary>
        /// label_map (全項目) のパーサー
        ///     参考 : https://www.ipentec.com/document/csharp-sprache-simple-multi-section-and-multi-key-value-parser
        /// </summary>
        public static readonly Parser<clsLabelMapList> LabelMapListParser = (from list in clsLabelMapItem.LabelMapItemParser.Many()
                                                                             select new clsLabelMapList(list)).End();

        /// <summary>
        /// label_map.txt の読込み
        /// </summary>
        /// <param name="strFilePath">読込む label_map.txt ファイルのパス</param>
        /// <returns></returns>
        public bool ReadLabelMapTextFile(string strFilePath)
        {
            try
            {
                // 念のためファイルの存在チェック
                if (true != File.Exists(strFilePath)) return false;

                // label_map.txt ファイルの内容を一括読み込み
                string strFileContents = File.ReadAllText(strFilePath);

                // パーサーを使ってファイルの内容を解析
                clsLabelMapList list = LabelMapListParser.Parse(strFileContents);

                // 自分自身のメンバ変数にコピー
                this.prv_LabelMapList = list.prv_LabelMapList;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 指定クラスの display_name を取得
        /// </summary>
        /// <param name="id">クラスの指定</param>
        /// <returns>指定クラスの display_name </returns>
        public string GetDisplayName(int id)
        {
            try
            {
                if (null == prv_LabelMapList) return string.Empty;

                clsLabelMapItem[] FilteredList = prv_LabelMapList.Where(item => item.id == id).ToArray();

                if (0 == FilteredList.Length) return string.Empty;

                return FilteredList[0].display_name;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
