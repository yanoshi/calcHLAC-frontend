﻿using System;
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
            Scale = 1.0;
        }


        private List<Rectangle> rectList = new List<Rectangle>();



        #region Binding可能なScaleプロパティ
        private ScaleTransform scaleTransform = new ScaleTransform(1.0d, 1.0d);

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale",
                                        typeof(double),
                                        typeof(AreaSettingCanvas),
                                        new UIPropertyMetadata(
                                            1.0d,
                                            // PropertyChangedCallback
                                            (d, e) =>
                                            {
                                                // プロパティ変更時の処理
                                                // 新しい値をリソースのScaleTransformにセットする
                                                (d as AreaSettingCanvas).scaleTransform.ScaleX = (double)e.NewValue;
                                                (d as AreaSettingCanvas).scaleTransform.ScaleY = (double)e.NewValue;
                                            }));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set
            {
                SetValue(ScaleProperty, value);
                motherGrid.LayoutTransform = scaleTransform;
            }
        }

        #endregion


        #region メソッド

        private void Delete()
        {
            Yanoshi.CalcHLACGUI.Models.RectEx deleteObj = null;
            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            foreach (var oldRectEx in obj.GivenPictureData.CalcAreas)
            {
                if (oldRectEx.X <= obj.MouseX &&
                    oldRectEx.Y <= obj.MouseY &&
                    obj.MouseX < oldRectEx.X + oldRectEx.Width &&
                    obj.MouseY < oldRectEx.Y + oldRectEx.Height)
                {
                    deleteObj = oldRectEx;
                }
            }
            if (deleteObj != null)
                obj.GivenPictureData.CalcAreas.Remove(deleteObj);
        }
        #endregion



        #region イベント




        #region 選択領域上でのマウス関連イベント
        private bool inDrag = false;
        private double diffX;
        private double diffY;
        private int oldRectX, oldRectY;
        private Yanoshi.CalcHLACGUI.Models.RectEx nowActiveObj;
        private Rectangle nowMovingObj;
        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                //右クリックされた時は、領域お絵かきではなくスクロール
                return;
            }
            if(e.ClickCount == 2)
            {
                Delete();
                return;
            }

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
            if (e.ChangedButton == MouseButton.Right)
            {
                //右クリックされた時は、領域お絵かきではなくスクロール
                return;
            }
            inDrag = false;
            ((Rectangle)sender).ReleaseMouseCapture();

            if (nowMovingObj != null)
            {
                this.mainCanves.Children.Remove(nowMovingObj);
                nowMovingObj.ReleaseMouseCapture();
            }

            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            int x = (int)Canvas.GetLeft(nowMovingObj);
            int y = (int)Canvas.GetTop(nowMovingObj);
            int w = (int)nowMovingObj.Width;
            int h = (int)nowMovingObj.Height;

            obj.GivenPictureData.CalcAreas.Remove(nowActiveObj);
            obj.GivenPictureData.CalcAreas.Add(new Models.RectEx(x, y, w, h));

            nowMovingObj = null;
        }
        #endregion



        #region Grid上でのマウス関連イベント
        private bool isMouseDown = false;
        private Rectangle nowMakingObj;
        private double startX = 0, startY = 0;
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                //右クリックされた時は、領域お絵かきではなくスクロール
                return;
            }
            var obj = ((AreaSettingCanvesViewModel)this.DataContext);
            obj.IsMouseDown = false;

            if (isMouseDown)
            {

                Point pos = e.GetPosition(mainCanves);
                double w = pos.X - startX;
                double h = pos.Y - startY;

                this.mainCanves.Children.Remove(nowMakingObj);
                nowMakingObj.ReleaseMouseCapture();

                if (!(w <= 0 || h <= 0))
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
            if(e.ChangedButton==MouseButton.Right)
            {
                //右クリックされた時は、領域お絵かきではなくスクロール
                return;
            }
            

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



        #region スクロール実装用のイベント
        /*
         * この辺を参考に書いたよ☆
         * https://social.msdn.microsoft.com/Forums/ja-JP/1491fe0d-55b3-4e50-9171-7f834bac87fe?forum=wpfja
         */

        Point startPoint;
        Point startPosition;

        /// <summary>
        /// スクロールな実装(MouseMove)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ScrollViewer scrollViewer = (ScrollViewer)sender;
                Point point = e.GetPosition((ScrollViewer)sender);
                if (scrollViewer.PanningMode == PanningMode.VerticalFirst | scrollViewer.PanningMode == PanningMode.VerticalOnly)
                {
                    scrollViewer.ScrollToVerticalOffset(startPosition.Y + (point.Y - startPoint.Y) * -1);
                }
                else if (scrollViewer.PanningMode == PanningMode.HorizontalFirst | scrollViewer.PanningMode == PanningMode.HorizontalOnly)
                {
                    scrollViewer.ScrollToHorizontalOffset(startPosition.X + (point.X - startPoint.X) * -1);
                }
                else
                {
                    scrollViewer.ScrollToVerticalOffset(startPosition.Y + (point.Y - startPoint.Y) * -1);
                    scrollViewer.ScrollToHorizontalOffset(startPosition.X + (point.X - startPoint.X) * -1);
                }
            }
        }

        /// <summary>
        /// スクロールな実装(PreviewMouseRightButtonDown)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            startPoint = e.GetPosition((ScrollViewer)sender);

            ScrollViewer scrollViewer = (ScrollViewer)sender;
            startPosition = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
        }
        #endregion

        #endregion

    }
}
