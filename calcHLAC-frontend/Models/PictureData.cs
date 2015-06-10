using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

//OpenCV
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

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
            FileName=fileName;
        }

        public PictureData(Mat matObj)
        {
            Init();
            SetImage(matObj);
            FileName = "None";
        }

        public PictureData() { }


        /// <summary>
        /// 初期化用メソッド
        /// </summary>
        private void Init()
        {
            CalcAreas = new List<RectEx>();
            IsSeleced = false;
        }

        #endregion


        #region プロパティ
        /// <summary>
        /// HLAC演算座標を指定する
        /// </summary>
        public List<RectEx> CalcAreas { get; set; }

        public Mat Image { get; private set; }

        public System.Drawing.Bitmap MiniBitmap
        {
            get
            {
                var image = GetBitmap();
                int w = 100;
                int h = (int)((double)image.Height / ((double)image.Width / (double)w));

                var miniBmp = new System.Drawing.Bitmap(image, new System.Drawing.Size(w, h));
                image.Dispose();

                return miniBmp;
            }
        }

        public object MiniImageSource
        {
            get
            {
                var bmp = MiniBitmap;


                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
        }

        public String FileName { get; private set; }

        public bool IsSeleced { get; set; }
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
        private void SetImage(System.Drawing.Bitmap image)
        {
            this.Image = OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
        }

        /// <summary>
        /// Bitmapオブジェクトを得る
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Bitmap GetBitmap()
        {
            return this.Image.ToBitmap();
        }


        
        #endregion
    }
}
