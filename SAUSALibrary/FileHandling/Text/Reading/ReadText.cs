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
        private const string UNITY_NEW_POSITION = "EmbedTest\\ToGUI.csv";

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
        public static ObservableCollection<FullStackModel> ConvertCSVToStack(string fullCSVFilePath, string CSVFileName)
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();
            var fqCSVFileName = Path.Combine(fullCSVFilePath, CSVFileName);

            var lines = File.ReadLines(fqCSVFileName);

            foreach (string container in lines)
            {
                string[] FSMline = container.Split(',');
                modelList.Add(new FullStackModel(long.Parse(FSMline[0]), long.Parse(FSMline[1]), long.Parse(FSMline[2]), long.Parse(FSMline[3]), long.Parse(FSMline[4]), long.Parse(FSMline[5]), long.Parse(FSMline[6]), long.Parse(FSMline[7]), FSMline[8]));
            }

            return modelList;
        }

        /// <summary>
        /// Read from a CSV the ID and position values into a FullStackModel.
        /// 
        /// </summary>
        /// <param name="fullCSVFilePath"></param>
        /// <param name="CSVFileName"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        /// 
        public static ObservableCollection<FullStackModel> ConvertCSVToPositionStack()
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();

            var lines = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory.ToString() + UNITY_NEW_POSITION);


            foreach (string container in lines)
            {
                string[] FSMline = container.Split(',');
                modelList.Add(new FullStackModel(long.Parse(FSMline[0]), double.Parse(FSMline[1]), double.Parse(FSMline[2]), double.Parse(FSMline[3]), 0, 0, 0, 0, null));
            }

            return modelList;
        }


        ///<summary>
        ///Compare a Position Stack and a Full Stack and update the Full Stack
        /// </summary>
        ///<param name="PositionStack">FullStack with just ID and X,Y,Z position</param>
        ///<param name="ContainerStack">FullStack with all Containers listed currently</param>
        ///<returns>Observable Collection of FullStack</returns>
        ///

        public static ObservableCollection<FullStackModel> ComparePositionStackToCurrentStack (ObservableCollection<FullStackModel> PositionStack, ObservableCollection<FullStackModel> StackToUpdate)
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();
            foreach(var model in PositionStack)
            {
                foreach(var modelToUpdate in StackToUpdate)
                {
                    if (modelToUpdate.Index.Equals(model.Index))
                    {
                        modelList.Add(new FullStackModel(model.Index, model.XPOS, model.YPOS, model.ZPOS, modelToUpdate.Length, modelToUpdate.Width, modelToUpdate.Height, modelToUpdate.Weight, modelToUpdate.CrateName));
                    }
                }
            }
            return modelList;
        }
    }
}
