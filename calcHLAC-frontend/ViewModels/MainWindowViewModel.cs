using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Yanoshi.CalcHLACGUI.Models;
using Yanoshi.CalcHLACGUI.Common;
using Yanoshi.CalcHLACGUI.Calculator;

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

        #region PictureDatas
        /// <summary>
        /// 画像データ群
        /// </summary>
        public ObservableCollection<Models.PictureData> PictureDatas { get; set; }
        #endregion


        #region PictureFolderPath
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


        #region PictureDatasSelectedItem
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
                RaisePropertyChanged("PictureDatasSelectedIndex");
            }
        }
        #endregion


        #region PictureDatasSelectedIndex
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
        #endregion


        #region PictureDatasSelectedItemVM
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
        #endregion


        #region SeparatingValue
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
        #endregion


        #region IsShowingBinaryPict
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
        #endregion


        #region UsingOtsuMethod
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


        #region StepSizeStr
        private string _StepSizeStr = "1";
        public string StepSizeStr
        {
            get { return _StepSizeStr; }
            set
            {
                if(_StepSizeStr != value)
                {
                    _StepSizeStr = value;
                    
                    List<int> intList = new List<int>();
                    try
                    {
                        string[] steps = value.Split(',');
                        foreach (string str in steps)
                        {
                            intList.Add(int.Parse(str));
                        }
                        _StepSize = intList.ToArray();
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }

                    RaisePropertyChanged("StepSizeStr");
                }
            }
        }
        #endregion


        #region StepSize
        private int[] _StepSize;
        public int[] StepSize
        {
            get
            {
                if (_StepSize == null)
                    _StepSize = new int[] {1 };
                return _StepSize;
            }
            set { _StepSize = value; }
        }
        #endregion


        #region Scale
        private double _Scale = 1.0;
        /// <summary>
        /// 画像表示の倍率を指定
        /// </summary>
        public double Scale
        {
            get { return _Scale; }
            set
            {
                if (_Scale != value && value > 0)
                {
                    _Scale = value;
                    RaisePropertyChanged("Scale");
                }
            }
        }
        #endregion


        #region Memo
        private string _Memo = "memo";
        /// <summary>
        /// メモ書き
        /// </summary>
        public string Memo
        {
            get { return _Memo; }
            set
            {
                if(_Memo!=value)
                {
                    _Memo = value;
                    RaisePropertyChanged("Memo");
                }
            }
        }
        #endregion

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
                PictureDatasSelectedItem.IsSelected = true;
            }
            else if((PictureData)item != PictureDatasSelectedItem)
            {
                PictureDatasSelectedItem.IsSelected = false;
                ((PictureData)item).IsSelected = true;
                PictureDatasSelectedItem = ((PictureData)item);
            }
            RaisePropertyChanged("PictureDatasSelectedItem");
            RaisePropertyChanged("PictureDatasSelectedIndex");

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
                RaisePropertyChanged("PictureDatasSelectedIndex");
                RaisePropertyChanged("Scale");
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
                RaisePropertyChanged("PictureDatasSelectedIndex");
                RaisePropertyChanged("Scale");
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

            var obj = new SettingsForSave(PictureDatas)
            {
                SeparatingValue=this.SeparatingValue,
                IsShowingBinaryPict=this.IsShowingBinaryPict,
                UsingOtsuMethod=this.UsingOtsuMethod,
                Memo=this.Memo
            };

            obj.Save(fileName);
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

           
            this.PictureDatas.Clear();
            var obj = SettingsForSave.Load(fileName);
            this.PictureDatas = obj.GetPictureDatas();
            this.SeparatingValue = obj.SeparatingValue;
            this.IsShowingBinaryPict = obj.IsShowingBinaryPict;
            this.UsingOtsuMethod = obj.UsingOtsuMethod;
            this.Memo = obj.Memo;


            this.RaisePropertyChanged("SeparatingValue");
            this.RaisePropertyChanged("IsShowingBinaryPict");
            this.RaisePropertyChanged("UsingOtsuMethod");
            this.RaisePropertyChanged("PictureDatas");
            this.RaisePropertyChanged("Memo");
            this.RaisePropertyChangeForImages();
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


        #region CalcHLACCommand
        private void CalcHLAC()
        {
            string fileName;

            using (var diag = new fm.SaveFileDialog())
            {
                diag.Filter = "CSVファイル|*.csv";
                var result = diag.ShowDialog();
                if (result == fm.DialogResult.OK)
                    fileName = diag.FileName;
                else
                    return;
            }


            StringBuilder strBuilder = new StringBuilder();

            foreach(var obj in PictureDatas)
            { 
                foreach(var rect in obj.CalcAreas)
                {
                    RectAndFeature features;
                    features = HLACCalculator.GetFeatures(obj.GetBinaryMat(), rect, StepSize);
                    strBuilder.AppendLine(string.Join(",", features.Features));
                }
            }

            StreamWriter stw = new StreamWriter(fileName);
            stw.Write(strBuilder.ToString());
            stw.Close();
            
        }
        private RelayCommand _CalcHLAC;
        public RelayCommand CalcHLACCommand
        {
            get
            {
                if (_CalcHLAC == null)
                    _CalcHLAC = new RelayCommand(CalcHLAC);

                return _CalcHLAC;
            }
        }
        #endregion


        #region CallGCCommand
        private void CallGC()
        {
            GC.Collect();
        }
        private RelayCommand _CallGC;
        public RelayCommand CallGCCommand
        {
            get
            {
                if (_CallGC == null)
                    _CallGC = new RelayCommand(CallGC);
                return _CallGC;
            }
        }
        #endregion


        #region SaveAreaInformationCommand
        private void SaveAreaInformation()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendFormat("{0},{1},{2},{3},{4}\n",
                "filename",
                "x",
                "y",
                "width",
                "height");

            foreach(var pict in PictureDatas)
            {
                string fileName = pict.FileName;

                foreach(var calcArea in pict.CalcAreas)
                {
                    strb.AppendFormat("{0},{1},{2},{3},{4}\n",
                        fileName,
                        calcArea.X,
                        calcArea.Y,
                        calcArea.Width,
                        calcArea.Height);
                }
            }



            string saveFileName;

            using (var diag = new fm.SaveFileDialog())
            {
                diag.Filter = "CSVファイル|*.csv";
                var result = diag.ShowDialog();
                if (result == fm.DialogResult.OK)
                    saveFileName = diag.FileName;
                else
                    return;
            }

            var stw = new StreamWriter(saveFileName);
            stw.Write(strb.ToString());
            stw.Close();
        }
        private RelayCommand _SaveAreaInfomation;
        public RelayCommand SaveAreaInformationCommand
        {
            get
            {
                if (_SaveAreaInfomation == null)
                    _SaveAreaInfomation = new RelayCommand(SaveAreaInformation);
                return _SaveAreaInfomation;
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
            RaisePropertyChanged("Scale");
        }
        #endregion
    }
}
