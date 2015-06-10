using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Yanoshi.CalcHLACGUI.Models;
using Yanoshi.CalcHLACGUI.Common;

namespace Yanoshi.CalcHLACGUI.ViewModels
{
    using fm = System.Windows.Forms;
    using System.Collections.ObjectModel;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            //とりあえずデバッグ用の処理を書いとくよ
            this.PictureDatas = new ObservableCollection<PictureData>
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
        /// <summary>
        /// 画像データ群
        /// </summary>
        public ObservableCollection<Models.PictureData> PictureDatas { get; set; }


        private String _PictureFolderPath;
        /// <summary>
        /// 画像が格納されているフォルダパスの情報を格納するプロパティ
        /// </summary>
        public String PictureFolderPath 
        { 
            get
            {
                return _PictureFolderPath;
            }
            set
            {
                if(PictureFolderPath != value)
                {
                    if (!Directory.Exists(value))
                        return;

                    _PictureFolderPath = value;
                    RaisePropertyChanged("PictureFolderPath");

                    LoadImages(value);
                    //RaisePropertyChanged("PictureDatas");
                }
            }
        }


        
        #endregion

        #region コマンド

        private void FolderSelectionDialog()
        {
            using (var diag = new fm.FolderBrowserDialog())
            {
                diag.SelectedPath = this.PictureFolderPath;
                var result = diag.ShowDialog();
                if (result == fm.DialogResult.OK)
                    PictureFolderPath = diag.SelectedPath;
            }
        }
        private RelayCommand _FolderSelectionDialog;
        /// <summary>
        /// フォルダを開くためのコマンド
        /// </summary>
        public RelayCommand FolderSelectionDialogCommand
        {
            get
            {
                if (_FolderSelectionDialog == null)
                    _FolderSelectionDialog = new RelayCommand(FolderSelectionDialog);
                return _FolderSelectionDialog;
            }
        }
        
        #endregion



        #region メソッド
        /// <summary>
        /// 画像をフォルダから読み込む処理
        /// </summary>
        /// <param name="folderPath"></param>
        private void LoadImages(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            PictureDatas.Clear();
            
            foreach(string fileName in files)
            {
                PictureDatas.Add(new PictureData(fileName));
            }
        }
        #endregion
    }
}
