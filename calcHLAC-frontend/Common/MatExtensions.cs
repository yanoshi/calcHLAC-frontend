using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp.CPlusPlus;


namespace Yanoshi.CalcHLACGUI.Common
{
    static class MatExtensions
    {
        public static byte GetPixel(this Mat obj,int x,int y)
        {

            return obj.At<byte>(y, x);
        }
    }
}
