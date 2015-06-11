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
    /// PictureDataをそのままシリアライズすると、Matが邪魔をするので、保存したいデータのみを集めた基底クラスを作りましたとさ。
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


        public String FileName { get; protected set; }

        public bool IsSeleced { get; set; }

        public bool IsBinaryOutputMode { get; set; }

        public int BinaryThreshold { get; set; }

        public bool UsingOtsuMethod { get; set; }


        virtual public Bitmap ImageForSave { get; set; }
        #endregion



        #region 保存用のメソッド群

        virtual public void RunSettingForSaving()
        {
            ImageForSave = Image.ToBitmap();
        }

        virtual public void RubSettingForLoaded()
        {
            this.Image = ImageForSave.ToMat();
        }
        #endregion
    }
}
