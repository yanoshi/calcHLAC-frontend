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
using System.Collections.ObjectModel;

namespace Yanoshi.CalcHLACGUI.Models
{
    /// <summary>
    /// ロードした画像データの管理
    /// </summary>
    [Serializable()]
    public class PictureData : PictureDataBase
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
            CalcAreas = new ObservableCollection<RectEx>();
            IsSeleced = false;
            IsBinaryOutputMode = false;
        }

        #endregion


        #region プロパティ
        
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


        public object ImageSource
        {
            get
            {
                var bmp = GetBitmap();


                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
        }

        #endregion


        #region メソッド
        /// <summary>
        /// 画像を設定する
        /// </summary>
        /// <param name="image">Matオブジェクト</param>
        private void SetImage(Mat image)
        {
            var obj = new Mat();

            Cv2.CvtColor(image, obj, ColorConversion.RgbaToGray);

            this.Image = obj;
            image.Dispose();
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
            if(IsBinaryOutputMode)
            {
                Mat output = new Mat();
                

                if(UsingOtsuMethod)
                    Cv2.Threshold(this.Image, output, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
                else
                    Cv2.Threshold(this.Image, output, BinaryThreshold, 255, ThresholdType.Binary);

                return output.ToBitmap();
            }
            else
                return this.Image.ToBitmap();
        }


        
        #endregion


    }
}
