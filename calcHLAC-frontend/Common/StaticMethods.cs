using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Yanoshi.CalcHLACGUI.Models;
using System.Collections.ObjectModel;

namespace Yanoshi.CalcHLACGUI.Common
{
    public static class StaticMethods
    {


        /// <summary>
        /// バイナリファイルからロードしてPictureDataオブジェクトを新規で作成します
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static object LoadFromBinaryFile(string fileName)
        {
            FileStream fs = new FileStream(fileName,
               FileMode.Open,
               FileAccess.Read);
               BinaryFormatter f = new BinaryFormatter();
            //読み込んで逆シリアル化する
            object obj = f.Deserialize(fs);
            fs.Close();

            return obj;
        }


        /// <summary>
        /// オブジェクトをバイナリファイルに保存する
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void SaveToBinaryFile(string fileName,object obj)
        {
            FileStream fs = new FileStream(fileName,
                FileMode.Create,
                FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            //シリアル化して書き込む
            bf.Serialize(fs, obj);
            fs.Close();
        }


        public static object DeepCopy(this object target)
        {

            object result;
            BinaryFormatter b = new BinaryFormatter();

            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;

        }
    }



}
