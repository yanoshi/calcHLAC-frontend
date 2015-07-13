using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yanoshi.CalcHLACGUI.Models;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using Yanoshi.CalcHLACGUI.Common;

namespace Yanoshi.CalcHLACGUI.Calculator
{
    /// <summary>
    /// HLACを演算するためのクラス
    /// </summary>
    public static class HLACCalculator
    {

        public static RectAndFeature GetFeatures(Mat matObj, RectEx rect, int[] stepSizes)
        {
            RectAndFeature returnRect = new RectAndFeature(rect);

            //探索開始点
            int start_x = rect.X + 1;
            int start_y = rect.Y + 1;

            //探索終了点
            int end_x = rect.X + rect.Width - 2;
            int end_y = rect.Y + rect.Height - 2;

            
            foreach(int step in stepSizes)
            {
                returnRect.Features.AddRange(CalcFeatures(matObj, start_x, start_y, end_x, end_y, step));
            }


            //正規化します
            double allFeaturesAddition = 0;
            for (int i = 0; i < returnRect.Features.Count;i++ )
            {
                allFeaturesAddition += Math.Pow(returnRect.Features[i], 2.0);
            }
            double scalar = Math.Sqrt(allFeaturesAddition);
            for (int i = 0; i < returnRect.Features.Count; i++)
            {
                returnRect.Features[i] /= scalar;
            }


            return returnRect;
        }


        private static double[] CalcFeatures(Mat matObj ,int start_x,int start_y,int end_x,int end_y,int step)
        {
            double[] returnData = new double[25];


            for (int iy = start_y; iy <= end_y; iy += step)
            {
                for (int ix = start_x; ix <= end_x; ix += step)
                {
                    byte p5 = matObj.GetPixel(ix, iy); 

                    if (p5 != 0)
                    {
                        byte p1 = matObj.GetPixel(ix - 1, iy - 1);
                        byte p2 = matObj.GetPixel(ix, iy - 1);
                        byte p3 = matObj.GetPixel(ix + 1, iy - 1);

                        byte p4 = matObj.GetPixel(ix - 1, iy);
                        byte p6 = matObj.GetPixel(ix + 1, iy);

                        byte p7 = matObj.GetPixel(ix - 1, iy + 1);
                        byte p8 = matObj.GetPixel(ix, iy + 1);
                        byte p9 = matObj.GetPixel(ix + 1, iy + 1);

                        returnData[0]++;


                        if (p1 != 0)
                        {
                            returnData[1]++;
                            if (p8 != 0) returnData[13]++;
                            if (p3 != 0) returnData[21]++;
                            if (p7 != 0) returnData[22]++;
                        }
                        if (p2 != 0)
                        {
                            returnData[2]++;
                            if (p8 != 0) returnData[7]++;
                            if (p7 != 0) returnData[11]++;
                            if (p9 != 0) returnData[12]++;
                            if (p6 != 0) returnData[17]++;
                        }
                        if (p3 != 0)
                        {
                            returnData[3]++;
                            if (p7 != 0) returnData[6]++;
                            if (p4 != 0) returnData[9]++;
                            if (p8 != 0) returnData[14]++;
                        }
                        if (p4 != 0)
                        {
                            returnData[4]++;
                            if (p6 != 0) returnData[5]++;
                            if (p9 != 0) returnData[10]++;
                            if (p2 != 0) returnData[18]++;
                            if (p8 != 0) returnData[19]++;
                        }
                        if (p6 != 0)
                        {
                            if (p7 != 0) returnData[15]++;
                            if (p1 != 0) returnData[16]++;
                            if (p8 != 0) returnData[20]++;
                        }
                        if (p9 != 0)
                        {
                            if (p1 != 0) returnData[8]++;
                            if (p7 != 0) returnData[23]++;
                            if (p3 != 0) returnData[24]++;
                        }
                    }

                }
            }


            return returnData;
        }
        
    }
}
