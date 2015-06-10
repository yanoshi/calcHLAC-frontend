using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yanoshi.CalcHLACGUI.Models;
using Yanoshi.CalcHLACGUI.ViewModels;

namespace Yanoshi.CalcHLACGUI.ViewModels
{
    public class AreaSettingCanvesViewModel : ViewModelBase
    {
        public AreaSettingCanvesViewModel()
        {

        }


        #region プロパティ

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
        #endregion



        
    }
}
