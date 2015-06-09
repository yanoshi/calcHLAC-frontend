using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//OpenCV
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace Yanoshi.CalcHLACGUI.Models
{
    public class PictureData
    {
        #region コンストラクタとか
        public PictureData(String fileName)
        {
            Mat obj = new Mat(fileName);
            Init();
            SetImage(obj);
        }

        public PictureData(Mat matObj)
        {
            Init();
            SetImage(matObj);
        }



        /// <summary>
        /// 初期化用メソッド
        /// </summary>
        private void Init()
        {
            CalcAreas = new List<Rectangle>();
        }

        #endregion



        #region プロパティ
        /// <summary>
        /// HLAC演算座標を指定する
        /// </summary>
        public List<Rectangle> CalcAreas { get; set; }

        public Mat Image { get; private set; }
        #endregion


        #region メソッド
        /// <summary>
        /// 画像を設定する
        /// </summary>
        /// <param name="image">Matオブジェクト</param>
        private void SetImage(Mat image)
        {
            this.Image = image;
        }

        /// <summary>
        /// 画像を設定する
        /// </summary>
        /// <param name="image">Bitmapオブジェクト</param>
        private void SetImage(Bitmap image)
        {
            this.Image = OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
        }

        /// <summary>
        /// Bitmapオブジェクトを得る
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(this.Image);
        }


        
        #endregion
    }
}
