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

            //var fqFilePath = Path.Combine(fullDBFilePath, "TestFile.csv");
            var fqFilePath = "\\EmbedTest\\ToUnity.csv";
            using (var textWriter = File.CreateText(fqFilePath))
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
            var fqFilePath = @"\EmbedTest\ToUnityRoom.csv";

            using (var textWriter = File.CreateText(fqFilePath))
            {
                for(int i = 0 ; i < roomDimensions.Length ; i++)
                {
                    textWriter.WriteLine(roomDimensions[i]);
                }
            }
        }
    }
}
