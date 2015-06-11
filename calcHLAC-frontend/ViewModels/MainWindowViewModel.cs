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

            /*
            this.PictureDatas = new ObservableCollection<PictureData>
            {
                new PictureData(@"C:\git\research_optics\Images\A1confocal_PFC_3nd2_normalized\00041.tif")
                {
                    CalcAreas=new ObservableCollection<RectEx>{
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(0,0),Size=new OpenCvSharp.CPlusPlus.Size(10,10)},
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(100,100),Size=new OpenCvSharp.CPlusPlus.Size(100,200)}
                    }
                },
                new PictureData(@"C:\git\research_optics\Images\A1confocal_PFC_3nd2_normalized\00042.tif")
                {
                    CalcAreas=new ObservableCollection<RectEx>{
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(0,0),Size=new OpenCvSharp.CPlusPlus.Size(10,10)},
                        new RectEx{Point=new OpenCvSharp.CPlusPlus.Point(20,20),Size=new OpenCvSharp.CPlusPlus.Size(10,10)}
                    }
                }
            };

            */

            this.PictureDatas = new ObservableCollection<PictureData>();
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



        private PictureData _PictureDatasSelectedItem;
        /// <summary>
        /// PictureDatasなリストでどれが選択されているかを示す
        /// </summary>
        public PictureData PictureDatasSelectedItem
        {
            get
            {
                return _PictureDatasSelectedItem;
            }
            set
            {
                if (_PictureDatasSelectedItem == value)
                    return;

                _PictureDatasSelectedItem = value;

                RaisePropertyChanged("PictureDatasSelectedItem");
                RaisePropertyChanged("PictureDatasSelectedItemVM");
            }
        }

        /// <summary>
        /// PictureDatasなリストで選択状態にあるやつのインデックス値を返す
        /// </summary>
        public int PictureDatasSelectedIndex
        {
            get
            {
                if (PictureDatasSelectedItem == null)
                    return -1;
                return PictureDatas.IndexOf(PictureDatasSelectedItem);
            }
            set
            {
                if (PictureDatas.Count > value )
                {
                    PictureDatasSelectedItem = PictureDatas[value];
                }
            }
        }


        /// <summary>
        /// DataContextに指定するためのPictureDatasSelectedItem
        /// </summary>
        public AreaSettingCanvesViewModel PictureDatasSelectedItemVM
        {
            get
            {
                return new AreaSettingCanvesViewModel() { GivenPictureData = PictureDatasSelectedItem };
            }
        }


        private int _SeparatingValue = 127;
        /// <summary>
        /// 二値化用のしきい値
        /// </summary>
        public int SeparatingValue
        {
            get { return _SeparatingValue; }
            set
            {
                if(_SeparatingValue!=value)
                {
                    _SeparatingValue = value;

                    foreach (var obj in PictureDatas)
                    {
                        obj.BinaryThreshold = value;
                    }
                    RaisePropertyChanged("SeparatingValue");
                    RaisePropertyChangeForImages();
                }
            }
        }


        private bool _IsShowingBinaryPict = false;
        /// <summary>
        /// 二値化した画像を表示するかどうか
        /// </summary>
        public bool IsShowingBinaryPict
        {
            get { return _IsShowingBinaryPict; }
            set
            {
                if(_IsShowingBinaryPict != value)
                {
                    _IsShowingBinaryPict = value;
                    foreach(var obj in PictureDatas)
                    {
                        obj.IsBinaryOutputMode = value;
                    }
                    RaisePropertyChanged("IsShowingBinaryPict");
                    RaisePropertyChangeForImages();                   
                }
            }
        }



        private bool _UsingOtsuMethod = false;
        /// <summary>
        /// 大津らの手法を利用して2値化を行うかどうか
        /// </summary>
        public bool UsingOtsuMethod
        {
            get { return _UsingOtsuMethod; }
            set
            {
                if(_UsingOtsuMethod != value)
                {
                    _UsingOtsuMethod = value;
                    foreach(var obj in PictureDatas)
                    {
                        obj.UsingOtsuMethod = value;
                    }
                    RaisePropertyChanged("UsingOtsuMethod");
                    RaisePropertyChangeForImages();
                }
            }
        }

        #endregion





        #region コマンド


        #region FolderSelectionDialogCommand
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


        #region PictureDataListItemSelectCommand
        private void PictureDataListItemSelect(object item)
        {
            if (PictureDatasSelectedItem == null)
            {
                PictureDatasSelectedItem = ((PictureData)item);
                PictureDatasSelectedItem.IsSeleced = true;
            }
            else if((PictureData)item != PictureDatasSelectedItem)
            {
                PictureDatasSelectedItem.IsSeleced = false;
                ((PictureData)item).IsSeleced = true;
                PictureDatasSelectedItem = ((PictureData)item);
            }
            RaisePropertyChanged("PictureDatasSelectedItem");

        }
        private RelayCommand<object> _PictureDataListItemSelect;
        /// <summary>
        /// PictureDatasなリストの要素を選択した際に発生する
        /// </summary>
        public RelayCommand<object> PictureDataListItemSelectCommand
        {
            get
            {
                if (_PictureDataListItemSelect == null)
                    _PictureDataListItemSelect = new RelayCommand<object>(PictureDataListItemSelect);
                return _PictureDataListItemSelect;
            }
        }
        #endregion


        #region NextPictureCommand
        private void NextPicture()
        {
            if(this.PictureDatasSelectedIndex - 1 < this.PictureDatas.Count)
            {
                this.PictureDatasSelectedIndex++;
            }
        }
        private RelayCommand _NextPicture;
        public RelayCommand NextPictureCommand
        {
            get
            {
                if (_NextPicture == null)
                    _NextPicture = new RelayCommand(NextPicture);

                return _NextPicture;
            }
        }
        #endregion


        #region PrevPictureCommand
        private void PrevPicture()
        {
            if (this.PictureDatasSelectedIndex > 0)
            {
                this.PictureDatasSelectedIndex--;
            }
        }
        private RelayCommand _PrevPicture;
        public RelayCommand PrevPictureCommand
        {
            get
            {
                if (_PrevPicture == null)
                    _PrevPicture = new RelayCommand(PrevPicture);

                return _PrevPicture;
            }
        }
        #endregion


        #region SaveAllSettingsCommand
        private void SaveAllSettings()
        {
            //設定の保存をするぞい
            //バイナリも含んでいるので、バイナリシリアル化
            string fileName;

            using (var diag = new fm.SaveFileDialog())
            {
                diag.Filter = "全てのファイル|*";
                var result = diag.ShowDialog();
                if (result == fm.DialogResult.OK)
                    fileName = diag.FileName;
                else
                    return;
            }


            List<PictureDataBase> pictListForSave = new List<PictureDataBase>();

            foreach(var obj in PictureDatas)
            {
                PictureDataBase saveObj = (PictureDataBase)obj;
                saveObj.RunSettingForSaving();
                pictListForSave.Add(saveObj);
            }

            StaticMethods.SaveToBinaryFile(fileName, pictListForSave);

        }
        private RelayCommand _SaveAllSettings;
        public RelayCommand SaveAllSettingsCommand
        {
            get
            {
                if (_SaveAllSettings == null)
                    _SaveAllSettings = new RelayCommand(SaveAllSettings);

                return _SaveAllSettings;
            }
        }
        #endregion


        #region LoadAllSettingsCommand
        private void LoadAllSettings()
        {
            //設定の読み込みをするぞい
            string fileName;

            using (var diag = new fm.OpenFileDialog())
            {
                diag.Filter = "すべてのファイル|*";
                var result = diag.ShowDialog();
                if (result == fm.DialogResult.OK)
                    fileName = diag.FileName;
                else
                    return;
            }

            List<PictureDataBase> loadedData = (List<PictureDataBase>)StaticMethods.LoadFromBinaryFile(fileName);
            this.PictureDatas.Clear();
            foreach(var obj in loadedData)
            {
                obj.RubSettingForLoaded();
                this.PictureDatas.Add((PictureData)obj);
            }
        }
        private RelayCommand _LoadAllSettings;
        public RelayCommand LoadAllSettingsCommand
        {
            get
            {
                if (_LoadAllSettings == null)
                    _LoadAllSettings = new RelayCommand(LoadAllSettings);
                return _LoadAllSettings;
            }
        }
        #endregion
        

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
                PictureDatas.Add(new PictureData(fileName)
                    {
                        UsingOtsuMethod = this.UsingOtsuMethod,
                        IsBinaryOutputMode = this.IsShowingBinaryPict,
                        BinaryThreshold = SeparatingValue
                    });
            }
        }

        /// <summary>
        /// どのプロパティに関するRaisePropertyChangeを呼び出せばいいか忘れそうなので
        /// </summary>
        private void RaisePropertyChangeForImages()
        {
            RaisePropertyChanged("MiniImageSource");
            RaisePropertyChanged("PictureDatasSelectedItemVM");
        }
        #endregion
    }
}
