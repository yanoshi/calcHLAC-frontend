using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yanoshi.CalcHLACGUI.Common;

namespace Yanoshi.CalcHLACGUI.Models
{
    /// <summary>
    /// データを保存するためだけに作ったクラスです
    /// </summary>
    [Serializable()]
    public class SettingsForSave
    {
        public List<PictureDataBase> PictureDatas{ get; set;}
        public int SeparatingValue { get; set; }
        public bool IsShowingBinaryPict { get; set; }
        public bool UsingOtsuMethod { get; set; }
        public int[] StepSizes { get; set; }


        public SettingsForSave(ObservableCollection<PictureData> pictureDatas)
        {
            PictureDatas = new List<PictureDataBase>();

            foreach (var obj in pictureDatas)
            {
                PictureDataBase saveObj = (PictureDataBase)obj;
                saveObj.RunSettingForSaving();
                PictureDatas.Add(saveObj);
            }
        }

        public void Save(string fileName)
        { 
            StaticMethods.SaveToBinaryFile(fileName, this);
        }

        public ObservableCollection<PictureData> GetPictureDatas()
        {
            var returnObj = new ObservableCollection<PictureData>();
            foreach (var obj in PictureDatas)
            {
                obj.RubSettingForLoaded();
                returnObj.Add((PictureData)obj);
            }
            return returnObj;
        }

        public static SettingsForSave Load(string fileName)
        {
            return (SettingsForSave)StaticMethods.LoadFromBinaryFile(fileName);
        }
    }
}
