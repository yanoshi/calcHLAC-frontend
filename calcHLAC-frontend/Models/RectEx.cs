using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp.CPlusPlus;

namespace Yanoshi.CalcHLACGUI.Models
{
    /// <summary>
    /// 領域情報を適当に扱うための構造体
    /// </summary>
    public struct RectEx
    {
        #region メンバ変数
        private int width, height, x, y;
        #endregion


        public RectEx(Point p, Size s)
        {
            width = 0;
            height = 0;
            x = 0;
            y = 0;
            SetPoint(p);
            SetSize(s);
        }

        public Size Size 
        {
            get
            {
                return new Size(width,height);
            }
            set
            {
                SetSize(value);
            }
        }


        public Point Point
        {
            get
            {
                return new Point(x, y);
            }
            set
            {
                SetPoint(value);
            }
        }
        
        public override String ToString()
        {
            return String.Format("{0}-{1}-{2}-{3}", x, y, width, height);
        }



        private void SetSize(Size s)
        {
            width = s.Width;
            height = s.Height;
        }
        private void SetPoint(Point p)
        {
            x = p.X;
            y = p.Y;
        }
    }

}
