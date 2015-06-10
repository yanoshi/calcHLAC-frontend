using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Yanoshi.CalcHLACGUI.ViewModels;
using Yanoshi.CalcHLACGUI.Common;

namespace Yanoshi.CalcHLACGUI.Views
{
    /// <summary>
    /// AreaSettingCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class AreaSettingCanvas : UserControl
    {
        public AreaSettingCanvas()
        {
            InitializeComponent();

            this.DataContextChanged += AreaSettingCanvas_DataContextChanged;
        }

        
        private List<Rectangle> rectList = new List<Rectangle>();

        private Rectangle _NowActiveObj;
        private Rectangle NowActiveObj
        {
            get
            {
                return _NowActiveObj;
            }
            set
            {
                if (_NowActiveObj != null)
                    _NowActiveObj.Opacity = 0.2;
                
                _NowActiveObj = value;

                if (value != null)
                    value.Opacity = 0.5;
            }
        }


        #region コマンド用メソッド

        private void Delete()
        {
            if(NowActiveObj != null)
            {
                NowActiveObj.ReleaseMouseCapture();
                this.mainCanves.Children.Remove(NowActiveObj);
                rectList.Remove(NowActiveObj);
                NowActiveObj = null;
                inDrag = false;
                isMouseDown = false;
            }
        } 
        #endregion




        #region イベント
        private void AreaSettingCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((AreaSettingCanvesViewModel)this.DataContext).DeleteCommand = new RelayCommand(Delete);
        }





        private bool inDrag = false;
        private double diffX;
        private double diffY;
        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inDrag = true;
            Point point = e.GetPosition(mainCanves);
            diffX = point.X - Canvas.GetLeft((Rectangle)sender);
            diffY = point.Y - Canvas.GetTop((Rectangle)sender);
            ((Rectangle)sender).CaptureMouse();
            NowActiveObj = (Rectangle)sender;
        }
        private void rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (inDrag)
            {
                Point pos = e.GetPosition(mainCanves);
                Canvas.SetLeft((Rectangle)sender, pos.X - diffX);
                Canvas.SetTop((Rectangle)sender, pos.Y - diffY);
            }
        }
        private void rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            inDrag = false;
            ((Rectangle)sender).ReleaseMouseCapture();

            diffX = 0;
            diffY = 0;
        }



        private bool isMouseDown = false;
        private Rectangle nowMakingObj;
        private double startX = 0, startY = 0;
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(isMouseDown)
            {
                Point pos = e.GetPosition(mainCanves);
                double w = pos.X - startX;
                double h = pos.Y - startY;

                if (w < 0 || h < 0)
                {
                    this.mainCanves.Children.Remove(nowMakingObj);
                    nowMakingObj.ReleaseMouseCapture();
                }
                else
                {
                    nowMakingObj.MouseDown += rectangle_MouseDown;
                    nowMakingObj.MouseMove += rectangle_MouseMove;
                    nowMakingObj.MouseUp += rectangle_MouseUp;
                    rectList.Add(nowMakingObj);

                    NowActiveObj = nowMakingObj;
                }

                isMouseDown = false;
            } 
        }

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point pos = e.GetPosition(mainCanves);

                double w = pos.X-startX;
                double h = pos.Y-startY;

                if (w < 0 || h < 0)
                    return;

                nowMakingObj.Width=w;
                nowMakingObj.Height=h;
            }
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (inDrag == false && ((AreaSettingCanvesViewModel)this.DataContext).GivenPictureData != null)
            {
                Point pos = e.GetPosition(mainCanves);

                Rectangle rect = new Rectangle();
                rectList.Add(rect);
                
                Canvas.SetLeft(rect, pos.X);
                Canvas.SetTop(rect, pos.Y);

                this.mainCanves.Children.Add(rect);

                
                startX = pos.X;
                startY = pos.Y;

                nowMakingObj = rect;

                isMouseDown = true;
            }
        }
        
        #endregion

       
    }
}
