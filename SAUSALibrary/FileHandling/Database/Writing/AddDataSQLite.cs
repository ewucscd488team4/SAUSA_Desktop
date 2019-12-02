using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;


namespace SAUSALibrary.FileHandling.Database.Writing
{
    public class AddDataSQLite
    {
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
    }
}
