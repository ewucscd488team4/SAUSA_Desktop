using SAUSALibrary.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace SAUSALibrary.FileHandling.Database.Reading
{
    public class ReadSQLite
    {
        private const string DEFAULT_EXTENSION = ".sqlite";

        /// <summary>
        /// Gets a truncated list of items in the project stack, index and name only. For use in the main window crate list ListBox.
        /// </summary>
        /// <param name="databaseFolder"></param>
        /// <param name="dataBaseFile"></param>
        /// <returns></returns>
        public static ObservableCollection<MiniStackModel> GetContainerListInfo(string databaseFolder, string dataBaseFile)
        {
            ObservableCollection<MiniStackModel> modelList = new ObservableCollection<MiniStackModel>();
            var dbFile = dataBaseFile + DEFAULT_EXTENSION;
            var fqFilePath = Path.Combine(databaseFolder, dbFile);            

            if (File.Exists(fqFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                //var query = "select ID, Name from " + fileNameSplit[0];

                SQLiteCommand command = new SQLiteCommand("select ID, Name from " + dataBaseFile, m_dbConnection);
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
        public static ObservableCollection<StackModel> GetEntireStack(string filePath, string dataBaseFile)
        {
            ObservableCollection<StackModel> modelList = new ObservableCollection<StackModel>();
            var dbFile = dataBaseFile + DEFAULT_EXTENSION;
            var fqFilePath = Path.Combine(filePath, dbFile);            

            if (File.Exists(fqFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand("select * from " + dataBaseFile, m_dbConnection);

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
                throw new FileNotFoundException("Database to read does not exist!");
            }

            return modelList;
        }

        /// <summary>
        /// Reads the current project SQLite database and returns the column header labels for the database        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ProjectDBFieldModel GetDatabaseFieldLabels(string filePath, string fileName)
        {
            ProjectDBFieldModel model = new ProjectDBFieldModel();
            var dbFile = fileName + DEFAULT_EXTENSION;
            var fqFilePath = Path.Combine(filePath, dbFile);            

            if (File.Exists(fqFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand("select * from " + fileName, m_dbConnection);

                dritem = command.ExecuteReader();
                
                //TODO read fields from database with fields outside of the defaults

                //reads default column fields
                model.length = dritem.GetName(4);
                model.width = dritem.GetName(5);
                model.height = dritem.GetName(6);
                model.weight = dritem.GetName(7);
                model.name = dritem.GetName(8);

            } else
            {
                throw new FileNotFoundException("Database to read does not exist!");
            }
            return model;            
        }
    }
}
