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
            if (NowActiveObj != null)
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
        private int oldRectX, oldRectY;
        private Yanoshi.CalcHLACGUI.Models.RectEx nowActiveObj;
        private Rectangle nowMovingObj;
        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inDrag = true;

            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            nowMovingObj = new Rectangle();
            

            foreach(var oldRectEx in obj.GivenPictureData.CalcAreas)
            {
                if(oldRectEx.X <= obj.MouseX && 
                    oldRectEx.Y <= obj.MouseY &&
                    obj.MouseX < oldRectEx.X + oldRectEx.Width &&
                    obj.MouseY < oldRectEx.Y + oldRectEx.Height)
                {
                    oldRectX = oldRectEx.X;
                    oldRectY = oldRectEx.Y;
                    nowActiveObj = oldRectEx;
                }
            }


            Canvas.SetTop(nowMovingObj, oldRectY);
            Canvas.SetLeft(nowMovingObj, oldRectX);
            nowMovingObj.Width = ((Rectangle)sender).Width;
            nowMovingObj.Height = ((Rectangle)sender).Height;

            ((Rectangle)sender).Opacity = 0;

            this.mainCanves.Children.Add(nowMovingObj);

            diffX = obj.MouseX - oldRectX;
            diffY = obj.MouseY - oldRectY;
            ((Rectangle)sender).CaptureMouse();
        }
        private void rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            if (inDrag)
            {
                Canvas.SetLeft(nowMovingObj, obj.MouseX - diffX);
                Canvas.SetTop(nowMovingObj, obj.MouseY - diffY);
            }
        }
        private void rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            inDrag = false;
            ((Rectangle)sender).ReleaseMouseCapture();

            this.mainCanves.Children.Remove(nowMovingObj);
            nowMovingObj.ReleaseMouseCapture();
            
            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            int x = (int)Canvas.GetLeft(nowMovingObj);
            int y = (int)Canvas.GetTop(nowMovingObj);
            int w = (int)nowMovingObj.Width;
            int h = (int)nowMovingObj.Height;

            obj.GivenPictureData.CalcAreas.Remove(nowActiveObj);
            obj.GivenPictureData.CalcAreas.Add(new Models.RectEx(x, y, w, h));

            nowMovingObj = null;
        }



        private bool isMouseDown = false;
        private Rectangle nowMakingObj;
        private double startX = 0, startY = 0;
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            obj.IsMouseDown = false;

            if (isMouseDown)
            {

                Point pos = e.GetPosition(mainCanves);
                double w = pos.X - startX;
                double h = pos.Y - startY;

                this.mainCanves.Children.Remove(nowMakingObj);
                nowMakingObj.ReleaseMouseCapture();

                if (!(w < 0 || h < 0))
                {
                    //オブジェクト作成処理
                    obj.GivenPictureData.CalcAreas.Add(new Models.RectEx((int)startX, (int)startY, (int)w, (int)h));
                }

                isMouseDown = false;
            }
        }

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {

            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            Point pos = e.GetPosition(mainCanves);

            if (isMouseDown)
            {

                double w = pos.X - startX;
                double h = pos.Y - startY;

                if (w < 0 || h < 0)
                    return;

                nowMakingObj.Width = w;
                nowMakingObj.Height = h;
            }


            obj.MouseX = pos.X;
            obj.MouseY = pos.Y;
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var obj = ((AreaSettingCanvesViewModel)this.DataContext); 
            obj.IsMouseDown = true;

            if (inDrag == false && obj.GivenPictureData != null)
            {
                Point pos = e.GetPosition(mainCanves);

                Rectangle rect = new Rectangle();

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
