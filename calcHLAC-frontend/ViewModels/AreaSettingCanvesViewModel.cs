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

        #endregion 
    }
}
