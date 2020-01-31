using SAUSALibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAUSALibrary.FileHandling.Text.Reading
{
    public class ReadText
    {
        public static bool ReadUnityCSV(string fqFilePath)
        {
            return false;
        }

        /// <summary>
        /// Read from a CSV into a FullStackModel.
        /// 
        /// </summary>
        /// <param name="fullCSVFilePath"></param>
        /// <param name="CSVFileName"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        /// 
        public static ObservableCollection<FullStackModel> ConvertCSVToStack (string fullCSVFilePath, string CSVFileName)
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();
            var fqCSVFileName = Path.Combine(fullCSVFilePath, CSVFileName);

            //StreamReader stream = new StreamReader(fqCSVFileName);
            //var lines = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\ToGUI.csv");
            var lines = File.ReadLines(fqCSVFileName);

            foreach (string container in lines)
            {
                string[] FSMline = container.Split(',');
                modelList.Add(new FullStackModel(long.Parse(FSMline[0]), long.Parse(FSMline[1]), long.Parse(FSMline[2]), long.Parse(FSMline[3]), long.Parse(FSMline[4]), long.Parse(FSMline[5]), long.Parse(FSMline[6]), long.Parse(FSMline[7]), FSMline[8]));
            }

            return modelList;
        }
    }
}
