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
        }

        private List<Rectangle> rectList = new List<Rectangle>();


        #region イベント
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
        private Rectangle nowActiveObj;
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
                    this.mainCanves.Children.Remove(nowActiveObj);
                    nowActiveObj.ReleaseMouseCapture();
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

                nowActiveObj.Width=w;
                nowActiveObj.Height=h;
            }
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (inDrag == false && ((AreaSettingCanvesViewModel)this.DataContext).GivenPictureData != null)
            {
                Point pos = e.GetPosition(mainCanves);

                Rectangle rect = new Rectangle();
                rectList.Add(rect);

                rect.Fill=Brushes.Green;
                
                
                Canvas.SetLeft(rect, pos.X);
                Canvas.SetTop(rect, pos.Y);

                this.mainCanves.Children.Add(rect);

                
                startX = pos.X;
                startY = pos.Y;

                nowActiveObj = rect;

                isMouseDown = true;
            }
        }
        
        #endregion

       
    }
}
