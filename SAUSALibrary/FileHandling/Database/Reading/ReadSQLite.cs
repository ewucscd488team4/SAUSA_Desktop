using SAUSALibrary.Models;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace SAUSALibrary.FileHandling.Database.Reading
{
    public class ReadSQLite
    {
        /// <summary>
        /// Gets a truncated list of items in the project stack, index and name only. For use in the main window crate list ListBox.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ObservableCollection<MiniStackModel> GetContainerListInfo(string filePath, string fileName)
        {
            ObservableCollection<MiniStackModel> modelList = new ObservableCollection<MiniStackModel>();
            var fqFilePath = Path.Combine(filePath, fileName);
            var fileNameSplit = fileName.Split('.');

            if (File.Exists(fqFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                //var query = "select ID, Name from " + fileNameSplit[0];

                SQLiteCommand command = new SQLiteCommand("select ID, Name from " + fileNameSplit[0], m_dbConnection);
                //command.Parameters.AddWithValue("@ID", ID); //to paremeterize

                dritem = command.ExecuteReader();

                while (dritem.Read())
                {
                    MiniStackModel model = new MiniStackModel
                    {
                        Index = (long)dritem["ID"],
                        CrateName = dritem["Name"].ToString()
                    };
                    modelList.Add(model);
                }

            }
            else
            {
                //TODO throw error dialog if passed in database file is not found
            }
            return modelList;
        }

        /// <summary>
        /// Reads and returns the entire project database for use in drawing the 3D GUI view window
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ObservableCollection<StackModel> GetEntireStack(string filePath, string fileName)
        {
            ObservableCollection<StackModel> modelList = new ObservableCollection<StackModel>();
            var fqFilePath = Path.Combine(filePath, fileName);
            var fileNameSplit = fileName.Split('.');

            if (File.Exists(fqFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand("select * from " + fileNameSplit[0], m_dbConnection);

                dritem = command.ExecuteReader();

                while (dritem.Read())
                {
                    StackModel model = new StackModel
                    {
                        Index = (long)dritem["ID"],
                        XPOS = (double)dritem["Xpos"],
                        YPOS = (double)dritem["Ypos"],
                        ZPOS = (double)dritem["Zpos"],
                        Length = (double)dritem["Length"],
                        Width = (double)dritem["Width"],
                        Height = (double)dritem["Height"],
                        Weight = (double)dritem["Weight"],
                        CrateName = dritem["Name"].ToString()
                    };

                    modelList.Add(model);
                }
            }
            else
            {
                //TODO throw error dialog if passed in file does is not found
            }

            return modelList;
        }
    }
}
