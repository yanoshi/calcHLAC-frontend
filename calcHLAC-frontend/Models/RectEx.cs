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
    [Serializable()]
    public class RectEx
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

        public RectEx(int x,int y,int width,int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public RectEx() { }

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


        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public string Str
        {
            get { return this.ToString(); }
        }
    }

}
