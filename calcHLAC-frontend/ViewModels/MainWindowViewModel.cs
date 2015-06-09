using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yanoshi.CalcHLACGUI.Models;

namespace Yanoshi.CalcHLACGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            //とりあえずデバッグ用の処理を書いとくよ
            this.PictureDatas = new List<PictureData>
            {
                new PictureData(@"C:\git\research_optics\Images\A1confocal_PFC_3nd2_normalized\00041.tif")
                {
                    CalcAreas=new List<RectEx>{
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(0,0),Size=new OpenCvSharp.CPlusPlus.Size(10,10)},
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(20,20),Size=new OpenCvSharp.CPlusPlus.Size(10,10)}
                    }
                },
                new PictureData(@"C:\git\research_optics\Images\A1confocal_PFC_3nd2_normalized\00042.tif")
                {
                    CalcAreas=new List<RectEx>{
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(0,0),Size=new OpenCvSharp.CPlusPlus.Size(10,10)},
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(20,20),Size=new OpenCvSharp.CPlusPlus.Size(10,10)}
                    }
                }
            };

        }


        #region プロパティ
        public List<Models.PictureData> PictureDatas { get; set; }
        #endregion
    }
}
