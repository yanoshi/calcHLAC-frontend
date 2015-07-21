using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System.Collections.ObjectModel;

namespace Yanoshi.CalcHLACGUI.Models
{
    /// <summary>
    /// PictureDataをそのままシリアライズすると、Matクラスが邪魔をするので、保存したいデータのみを集めた基底クラスを作りましたとさ。
    /// </summary>
    [Serializable()]
    public class PictureDataBase
    {
        #region プロパティ

        /// <summary>
        /// HLAC演算座標を指定する
        /// </summary>
        public ObservableCollection<RectEx> CalcAreas { get; set; }

        [NonSerialized()]
        private Mat _Image;
        public Mat Image
        {
            get { return _Image; }
            protected set { _Image = value; }
        }

        /// <summary>
        /// ファイル名
        /// </summary>
        public String FileName { get; protected set; }

        /// <summary>
        /// この項目が「選択状態」であるかを示す
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 二値化して出力するか否か
        /// </summary>
        public bool IsBinaryOutputMode { get; set; }

        /// <summary>
        /// 二値化時のしきい値
        /// </summary>
        public int BinaryThreshold { get; set; }

        /// <summary>
        /// 大津らの手法を利用して二値化を行うか否か
        /// </summary>
        public bool UsingOtsuMethod { get; set; }

        /// <summary>
        /// メディアンフィルタを利用するかどうか
        /// </summary>
        public bool UsingMedianBlur { get; set; }

        /// <summary>
        /// 保存用のオブジェクト(継承されてほしくなかったのでvirtual)
        /// </summary>
        virtual public Bitmap ImageForSave { get; set; }
        #endregion



        #region 保存用のメソッド群
        /// <summary>
        /// シリアライズする前にかならず呼びだそう
        /// </summary>
        virtual public void RunSettingForSaving()
        {
            ImageForSave = Image.ToBitmap();
        }

        /// <summary>
        /// デシリアライズ後に必ず呼びだそう
        /// </summary>
        virtual public void RubSettingForLoaded()
        {
            this.Image = ImageForSave.ToMat();
        }
        #endregion
    }
}
