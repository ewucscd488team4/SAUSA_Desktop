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
        /// <param name="dbFullFilePath"></param>
        /// <param name="dbFile"></param>
        /// <returns></returns>
        public static ObservableCollection<MiniStackModel> GetContainerListInfo(string dbFullFilePath, string dbFile)
        {
            ObservableCollection<MiniStackModel> modelList = new ObservableCollection<MiniStackModel>(); //collection to return
            
            string[] file = dbFile.Split('.'); //split the dbfile so we can use the file name with out the file extension

            if (File.Exists(dbFullFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + dbFullFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                var queryString = "select ID, Name from " + file[0];

                SQLiteCommand command = new SQLiteCommand(queryString, m_dbConnection);                

                dritem = command.ExecuteReader();

                while (dritem.Read())
                {                    
                    modelList.Add(new MiniStackModel((long)dritem["ID"], dritem["Name"].ToString()));
                }
                m_dbConnection.Close();
            }
            else
            {
                throw new FileNotFoundException("Database to read does not exist!");
            }
            
            return modelList;
        }

        /// <summary>
        /// Reads and returns the entire project database for use in drawing the 3D GUI view window
        /// </summary>
        /// <param name="dbFullFilePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ObservableCollection<FullStackModel> GetEntireStack(string dbFullFilePath, string dbFile)
        {
            ObservableCollection<FullStackModel> modelList = new ObservableCollection<FullStackModel>();
            string[] file = dbFile.Split('.');

            if (File.Exists(dbFullFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + dbFullFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand("select * from " + file[0], m_dbConnection);

                dritem = command.ExecuteReader();

                while (dritem.Read())
                {
                    modelList.Add(new FullStackModel(
                        (long)dritem["ID"],
                        (double)dritem["Xpos"],
                        (double)dritem["Ypos"],
                        (double)dritem["Zpos"],
                        (double)dritem["Length"],
                        (double)dritem["Width"],
                        (double)dritem["Height"],
                        (double)dritem["Weight"],
                        dritem["Name"].ToString()
                        )
                        );
                }
                m_dbConnection.Close();
            }
            else
            {
                throw new FileNotFoundException("Database to read does not exist!");
            }

            return modelList;
        }

        /// <summary>
        /// Reads the current project SQLite database and returns the column header labels for the database
        /// </summary>
        /// <param name="dbFullFilePath"></param>
        /// <param name="dbFile"></param>
        /// <returns></returns>
        public static ProjectDBFieldModel GetDatabaseFieldLabels(string dbFullFilePath, string dbFile)
        {
            ProjectDBFieldModel model = new ProjectDBFieldModel();

            string[] file = dbFile.Split('.');

            if (File.Exists(dbFullFilePath))
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + dbFullFilePath + ";Version=3;");
                SQLiteDataReader dritem = null;
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand("select * from " + file[0], m_dbConnection);

                dritem = command.ExecuteReader();
                
                //TODO read fields from database with fields outside of the defaults

                //reads default column fields
                model.length = dritem.GetName(4);
                model.width = dritem.GetName(5);
                model.height = dritem.GetName(6);
                model.weight = dritem.GetName(7);
                model.name = dritem.GetName(8);
                m_dbConnection.Close();
            } else
            {
                throw new FileNotFoundException("Database to read does not exist!");
            }
            return model;            
        }
    }
}
