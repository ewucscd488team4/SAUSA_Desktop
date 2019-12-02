using System.Data.SQLite;
using System.IO;
using System.Text;


namespace SAUSALibrary.FileHandling.Database.Writing
{
    public class CreateNewSQLiteDatabase
    {
        //TODO expand design of creating a new database to take a list of type string for column headers
        //default database fiels, will later bring this in via a List of type string, for more flexibile database design.
        private static string[] defaultFields = { "Length", "Width", "Height", "Weight", "Xpos", "Ypos", "Zpos", "Name" };

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

            SQLiteConnection.CreateFile(fqFilePath); //creates a file (that is empty)

            //defines the SQLite connection and opens it
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
            m_dbConnection.Open();

            //use stringbuider to build sql command.
            sb.Append("create table ");
            sb.Append(fileNameSplit[0]);
            sb.Append(" (ID INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.Append(defaultFields[4] + " REAL,");
            sb.Append(defaultFields[5] + " REAL,");
            sb.Append(defaultFields[6] + " REAL,");
            sb.Append(defaultFields[0] + " REAL,");
            sb.Append(defaultFields[1] + " REAL,");
            sb.Append(defaultFields[2] + " REAL,");
            sb.Append(defaultFields[3] + " REAL,");
            sb.Append(defaultFields[7] + " VARCHAR(80)");
            sb.Append(");");

            //defines the sql query command, and what database connection to execute it on, then runs it
            SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);
            command.ExecuteNonQuery();

            //do clean up
            sb.Clear();
            m_dbConnection.Close();
        }

    }
}
