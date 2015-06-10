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




        #region イベント
        private bool _inDrag = false;
        private double _diffX;
        private double _diffY;
        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _inDrag = true;
            Point point = e.GetPosition(mainCanves);
            _diffX = point.X - Canvas.GetLeft((Rectangle)sender);
            _diffY = point.Y - Canvas.GetTop((Rectangle)sender);
            ((Rectangle)sender).CaptureMouse();
        }
        private void rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_inDrag)
            {
                Point pos = e.GetPosition(mainCanves);
                Canvas.SetLeft((Rectangle)sender, pos.X - _diffX);
                Canvas.SetTop((Rectangle)sender, pos.Y - _diffY);
            }
        }
        private void rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _inDrag = false;
            ((Rectangle)sender).ReleaseMouseCapture();

            _diffX = 0;
            _diffY = 0;
        }

        #endregion
    }
}
