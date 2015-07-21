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
    /// 画像データの管理用クラス
    /// </summary>
    [Serializable()]
    public class PictureData : PictureDataBase
    {
        #region コンストラクタとか
        /// <summary>
        /// ファイルから初期化
        /// </summary>
        /// <param name="fileName"></param>
        public PictureData(String fileName)
        {
            Mat obj = new Mat(fileName);
            Init();
            SetImage(obj);
            FileName=fileName;
        }


        /// <summary>
        /// Matオブジェクトから初期化
        /// </summary>
        /// <param name="matObj"></param>
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
            IsSelected = false;
            IsBinaryOutputMode = false;
        }

        #endregion


        #region プロパティ
        
        /// <summary>
        /// 縮小されたBitmapを返します
        /// </summary>
        public System.Drawing.Bitmap MiniBitmap
        {
            get
            {
                using (var image = GetBitmap())
                {
                    int w = 100;
                    int h = (int)((double)image.Height / ((double)image.Width / (double)w));

                    var miniBmp = new System.Drawing.Bitmap(image, new System.Drawing.Size(w, h));
                    image.Dispose();

                    return miniBmp;
                }
            }
        }


        /// <summary>
        /// 縮小されたBitmapから作ったBitmapSourceオブジェクトを返します
        /// </summary>
        public object MiniImageSource
        {
            get
            {
                using (var bmp = MiniBitmap)
                {

                    return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        bmp.GetHbitmap(),
                        IntPtr.Zero,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
                }
            }
        }


        /// <summary>
        /// MatオブジェクトからBitmapSourceオブジェクトを作って返す
        /// </summary>
        public object ImageSource
        {
            get
            {
                using (var output = GetOutputMat())    
                { 
                    return output.ToBitmapSource();
                }
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
            using(var returnData = GetOutputMat())
            {
                return returnData.ToBitmap();
            }
        }


        private Mat GetOutputMat()
        {
            var output =  this.Image.Clone();


            if (UsingMedianBlur)
                output = output.MedianBlur(3);

            if (IsBinaryOutputMode)
            {
                if (UsingOtsuMethod)
                    Cv2.Threshold(output, output, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
                else
                    Cv2.Threshold(output, output, BinaryThreshold, 255, ThresholdType.Binary);
            }


            return output;
        }


        /// <summary>
        /// 二値化したMatを返す
        /// </summary>
        /// <returns></returns>
        public Mat GetBinaryMat()
        {
            Mat output = new Mat();

            if (UsingOtsuMethod)
                Cv2.Threshold(this.Image, output, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
            else
                Cv2.Threshold(this.Image, output, BinaryThreshold, 255, ThresholdType.Binary);

            return output;
        }
        #endregion
    }
}
