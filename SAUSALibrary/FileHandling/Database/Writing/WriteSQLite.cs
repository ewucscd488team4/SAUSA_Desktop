using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace SAUSALibrary.FileHandling.Database.Writing
{
    /// <summary>
    /// Class designed for writing to project sqlite databases
    /// </summary>
    public class WriteSQLite
    {

        //TODO expand design of creating a new database to take a list of type string for column headers
        //default database fiels, will later bring these values in via a List of type string, for more flexibile database design.
        private static string[] defaultFields = { "Length", "Width", "Height", "Weight", "Xpos", "Ypos", "Zpos", "Name" };

        /// <summary>
        /// Write a new line of data into the given project database. ValuesToEnter must be in x,y,z,L,W,H,Weight,Name order.
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="databaseFileName"></param>
        /// <param name="valuesToEnter"></param>
        public static void AddSQLiteData(string databasePath, string databaseFileName, List<string> valuesToEnter)
        {
            ///set up this way for testing, will make this method take two lists later
            List<string> defaultDatabaseFieldHeaders = new List<string>
            {
                "Xpos", "Ypos", "Zpos","Length", "Width", "Height", "Weight", "Name"
            };

            //split the file name into name plus extension
            var fileSplit = databaseFileName.Split('.');
            //combine file path with file name to make a full file path for 
            var fqFilePath = Path.Combine(databasePath, databaseFileName);

            //if db file exists
            if (File.Exists(fqFilePath))
            {
                StringBuilder sb = new StringBuilder();
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

                m_dbConnection.Open();

                //correctly formed statement example
                /*sb.Append("insert into ");
                sb.Append(fileSplit[0]);
                sb.Append(" (");
                for(int j = 0; j < defaultDatabaseFieldHeaders.Count -1; j++)
                {
                    sb.Append(defaultDatabaseFieldHeaders[j] +", ");
                }
                sb.Append(defaultDatabaseFieldHeaders[defaultDatabaseFieldHeaders.Count -1]);
                sb.Append(") values (");
                sb.Append("0, ");
                sb.Append("0, ");
                sb.Append("0, ");
                sb.Append("10, ");
                sb.Append("20, ");
                sb.Append("30, ");
                sb.Append("100, ");
                sb.Append("'Crate'");
                sb.Append(");");

                Console.WriteLine(sb.ToString());
                sb.Clear();*/

                //start of list formed statement
                sb.Append("insert into ");
                sb.Append(fileSplit[0]);
                sb.Append(" (");
                for (int x = 0; x < defaultDatabaseFieldHeaders.Count - 1; x++)
                {
                    sb.Append(defaultDatabaseFieldHeaders[x] + ", ");
                }
                sb.Append(defaultDatabaseFieldHeaders[defaultDatabaseFieldHeaders.Count - 1]);
                sb.Append(") values (");
                for (int i = 0; i < valuesToEnter.Count - 1; i++)
                {
                    sb.Append(valuesToEnter[i] + ", ");

                }
                sb.Append(valuesToEnter[valuesToEnter.Count - 1]);
                sb.Append(");");

                //Console.WriteLine(sb.ToString()); for testing
                sb.Clear(); //clear StringBuilder
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);
                command.ExecuteNonQuery(); //add data

                m_dbConnection.Close();

            }
            else
            {
                //TODO throw error dialog if database file does not exist
            }

        }

        /// <summary>
        /// Creates a new empty SQLite database with the file name specified and at the file path specified.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        public static void CreateDatabase(string filePath, string file)
        {
            var fqFilePath = Path.Combine(filePath, file); //cobines path and file into a full file path 
            var fileNameSplit = file.Split('.'); //splits the file name given into a 2 column array
            StringBuilder sb = new StringBuilder();

            SQLiteConnection.CreateFile(fqFilePath); //creates a new sqlite database file (that is empty, no tables at all)

            //defines the SQLite connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

            //opens the connection
            m_dbConnection.Open();

            //use stringbuider to build sql command.
            sb.Append("create table ");
            sb.Append(fileNameSplit[0]);
            sb.Append(" (ID INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.Append(defaultFields[4] + " REAL,"); //xpos
            sb.Append(defaultFields[5] + " REAL,"); //ypos
            sb.Append(defaultFields[6] + " REAL,"); //zpos
            sb.Append(defaultFields[0] + " REAL,"); //length
            sb.Append(defaultFields[1] + " REAL,"); //width
            sb.Append(defaultFields[2] + " REAL,"); //height
            sb.Append(defaultFields[3] + " REAL,"); //weight
            sb.Append(defaultFields[7] + " VARCHAR(80)"); //name
            sb.Append(");");

            //defines the sql query command, what database connection to execute it on
            SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);

            //runs command to build new table
            command.ExecuteNonQuery();

            //do clean up
            sb.Clear();
            m_dbConnection.Close();
        }

    }
}
