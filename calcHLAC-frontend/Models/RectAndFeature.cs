using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanoshi.CalcHLACGUI.Models
{
    /// <summary>
    /// 領域とそこから得られた特徴量をペアで管理するクラス
    /// </summary>
    public class RectAndFeature : RectEx
    {
        public RectAndFeature(RectEx rect)
        {
            this.Size = rect.Size;
            this.Point = rect.Point;
            Features = new List<double>();
        }


        #region プロパティ
        public List<double> Features { get; set; }
        #endregion
    }
}
