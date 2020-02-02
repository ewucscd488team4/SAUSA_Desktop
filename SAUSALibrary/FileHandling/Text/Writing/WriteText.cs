using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAUSALibrary.FileHandling.Text.Writing
{
    public class WriteText
    {
        private const string UNITY_DBCSV = @"\EmbedTest\ToUnity.csv";
        private const string UNITY_ROOMCSV = @"\EmbedTest\ToUnityRoom.csv";
        private const string UNITY_NEWCONTAINER = "ToGUI.csv";
        public static Boolean WriteUnityCSV(string fqFilePath)
        {
            return false;
        }

        /// <summary>
        /// Convert elements from IEnumerable
        /// </summary>
        /// <param name="fullDBFilePath"></param>
        /// <param name="dbFileName"></param>
        /// <param name="specifiedCustomDBFieldsList"></param>
        /// <returns></returns>
        /// 
        public static IEnumerable<string> ConvertToCSV<T>(IEnumerable<T> list)
        {
            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            foreach (var @object in list)
            {
                yield return string.Join(",",
                                         fields.Select(x => (x.GetValue(@object) ?? string.Empty).ToString())
                                               .Concat(properties.Select(p => (p.GetValue(@object, null) ?? string.Empty).ToString()))
                                               .ToArray());
            }
        }


        /// <summary>
        /// Write the database to a CSV using the GetEntireStack method and a ConvertToCSV helper method
        /// </summary>
        /// <param name="fullDBFilePath"></param>
        /// <param name="dbFileName"></param>
        /// <param name="specifiedCustomDBFieldsList"></param>
        /// <returns></returns>
        /// 
        public static void WriteDatabasetoCSV(string fullDBFilePath, string dbFileName)
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();
            string[] file = dbFileName.Split('.');

            modelList = ReadSQLite.GetEntireStack(fullDBFilePath, dbFileName);
                        
            using (var textWriter = File.CreateText(AppDomain.CurrentDomain.BaseDirectory.ToString() + UNITY_DBCSV))
            {
                foreach (var line in ConvertToCSV(modelList))
                {
                    textWriter.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Write room dimenions to CSV for unity to use.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="csvFileName"></param>
        /// <param name="roomDimensions"></param>
        public static void WriteRoomDimensionsToCSV(string[] roomDimensions)
        {            
            using (var textWriter = File.CreateText(AppDomain.CurrentDomain.BaseDirectory.ToString() + UNITY_ROOMCSV))
            {
                for(int i = 0 ; i < roomDimensions.Length ; i++)
                {
                    textWriter.WriteLine(roomDimensions[i]);
                }
            }
        }

        /// <summary>
        /// Writes the newly added container to a Unity "packet" text file
        /// </summary>
        /// <param name="FullwriteDirectory"></param>
        /// <param name="fileName"></param>
        /// <param name="modelToWrite"></param>
        public static void AddFullStackModeltoCSV(string FullwriteDirectory, FullStackModel modelToWrite)
        {
            //use UNITY_NEWCONTAINER constant for the file name

            //set up a string to write
            StringBuilder sb = new StringBuilder();
            sb.Append(modelToWrite.Index + ",");
            sb.Append(modelToWrite.XPOS + ",");
            sb.Append(modelToWrite.YPOS + ",");
            sb.Append(modelToWrite.ZPOS + ",");
            sb.Append(modelToWrite.Length + ",");
            sb.Append(modelToWrite.Width + ",");
            sb.Append(modelToWrite.Height + ",");
            sb.Append(modelToWrite.CrateName);

            //write to the file
            using (var textWriter = File.CreateText(AppDomain.CurrentDomain.BaseDirectory.ToString() + UNITY_NEWCONTAINER))
            {
                textWriter.WriteLine(sb);
            }
        }
    }
}
