using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yanoshi.CalcHLACGUI.Models;
using Yanoshi.CalcHLACGUI.ViewModels;

using Yanoshi.CalcHLACGUI.Common;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Yanoshi.CalcHLACGUI.ViewModels
{
    public class AreaSettingCanvesViewModel : ViewModelBase
    {
        public AreaSettingCanvesViewModel()
        {

        }



        #region メンバ変数
        public double startMouseX, startMouseY;

        #endregion



        #region プロパティ

        public double MouseX { get; set; }
        public double MouseY { get; set; }
        private bool _IsMouseDown = false;
        public bool IsMouseDown 
        { 
            get 
            {
                return _IsMouseDown;
            }
            set
            {
                if(_IsMouseDown!=value)
                {
                    if(value)
                    {
                        startMouseX = MouseX;
                        startMouseY = MouseY;
                    }
                }
            }
        }


        private PictureData _GivenPictureData;
        public PictureData GivenPictureData
        {
            get
            {
                return _GivenPictureData;
            }
            set
            {
                if(_GivenPictureData!=value)
                {
                    _GivenPictureData = value;
                    RaisePropertyChanged("GivenPictureData");
                }
            }
        }



        private List<Rectangle> _RectList = new List<Rectangle>();
        public List<Rectangle> RectList
        {
            get
            {
                return _RectList;
            }
            set
            {
                _RectList = value;
            }
        }


        /// <summary>
        /// RectListの内容をGivenPictureDataに反映させるメソッド
        /// </summary>
        public void RefreshAreaList()
        {
            if(RectList.Count == GivenPictureData.CalcAreas.Count)
            {
                //前回呼び出し時から新規で作られた領域はないものとして処理する
                for(int i=0;i<RectList.Count;i++)
                {
                    var rectEX = new RectEx(
                        new OpenCvSharp.CPlusPlus.Point(Canvas.GetLeft(RectList[i]), Canvas.GetTop(RectList[i])),
                        new OpenCvSharp.CPlusPlus.Size(RectList[i].Width, RectList[i].Height)
                        );

                    GivenPictureData.CalcAreas[i] = rectEX;
                }
            }
            else
            {
                GivenPictureData.CalcAreas.Clear();

                foreach(var obj in RectList)
                {
                    var rectEX = new RectEx(
                        new OpenCvSharp.CPlusPlus.Point(Canvas.GetLeft(obj), Canvas.GetTop(obj)),
                        new OpenCvSharp.CPlusPlus.Size(obj.Width, obj.Height)
                        );
                }
            }
            RaisePropertyChanged("CalcAreas");
        }
        #endregion



        #region コマンド
        private void DeleteKari() { }
        private RelayCommand _Delete;
        /// <summary>
        /// 選択範囲を示すRectangleを削除します
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_Delete == null)
                    _Delete = new RelayCommand(DeleteKari);

                return _Delete;
            }
            set
            {
                _Delete = value;
                RaisePropertyChanged("DeleteCommand");
            }
        }


        private void RectangleResize(RectEx obj)
        {
            
        }
        private RelayCommand<RectEx> _RectangleResize;
        public RelayCommand<RectEx> RectangleResizeCommand
        {
            get
            {
                if (_RectangleResize == null)
                    _RectangleResize = new RelayCommand<RectEx>(RectangleResize);
                return _RectangleResize;
            }
        }
        #endregion 
    }
}
