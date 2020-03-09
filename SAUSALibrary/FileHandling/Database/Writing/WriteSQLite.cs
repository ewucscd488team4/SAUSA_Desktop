using SAUSALibrary.Models;
using SAUSALibrary.Models.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const string DEFAULT_EXTENSION = ".sqlite";

        //TODO expand design of creating a new database to take a list of type string for column headers
        //default database fiels, will later bring these values in via a List of type string, for more flexibile database design.
        private static string[] defaultFields = { "Length", "Width", "Height", "Weight", "Xpos", "Ypos", "Zpos", "Name" };

        /// <summary>
        /// Write a new line of data into the given project database. ValuesToEnter must be in x,y,z,L,W,H,Weight,Name order.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        /// <param name="valuesToEnter"></param>
        public static void AddSQLiteData(string workingFolder, string dbFileName, List<string> valuesToEnter)
        {
            ///set up this way for testing, will make this method take two lists later
            List<string> defaultDatabaseFieldHeaders = new List<string>
            {
                "Xpos", "Ypos", "Zpos","Length", "Width", "Height", "Weight", "Name"
            };

            //split the file name into name plus extension
            var fileSplit = dbFileName.Split('.');
            //combine file path with file name to make a full file path for 
            var fqFilePath = Path.Combine(workingFolder, dbFileName);

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

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);
                command.ExecuteNonQuery(); //add data
                sb.Clear(); //clear StringBuilder
                m_dbConnection.Close();

            }
            else
            {
                throw new FileNotFoundException("Database file for writing does not exist!");
            }

        }

        /// <summary>
        /// Creates a new empty SQLite database with the file name specified and at the file path specified.
        /// New database will have a default table in the database using the given file name as the table name
        /// with minimum acceptable column headers.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        public static void CreateDefaultProjectDatabase(string workingFolder, string dbFileName)
        {            
            var fqFilePath = Path.Combine(workingFolder, dbFileName); //combines given save folder and file name into a fully qualified file path
            string[] file = dbFileName.Split('.');
                        
            StringBuilder sb = new StringBuilder();

            SQLiteConnection.CreateFile(fqFilePath); //creates a new sqlite database file (that is empty, no tables at all)

            //defines the SQLite connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

            //opens the connection
            m_dbConnection.Open();

            //use stringbuider to build sql command.
            sb.Append("create table ");
            sb.Append(file[0]);
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

            //defines the sql query command, and what database connection to execute it on
            SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);

            //runs command to build new table
            command.ExecuteNonQuery();

            //do clean up
            sb.Clear();
            m_dbConnection.Close();
        }
        
        /// <summary>
        /// Creates a project SQLite database file with nothing in it.
        /// </summary>
        /// <param name="workingFolder"></param>
        public static void CreateProjectDatabase(string workingFolder, string dbFileName)
        {
            var fqFilePath = Path.Combine(workingFolder, dbFileName);
            SQLiteConnection.CreateFile(fqFilePath);
        }

        /// <summary>
        /// Writes default project database table and minimum required SAUSA fields to the given project database, with default table being defined as the given file name minus given extension.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        public static void PopulateProjectDatabase(string workingFolder, string dbFileName)
        {
            StringBuilder sb = new StringBuilder();
            var fqFilePath = Path.Combine(workingFolder, dbFileName);
            string[] table = dbFileName.Split('.');

            //defines the SQLite connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

            //opens the connection
            m_dbConnection.Open();

            //use stringbuider to build sql command.
            sb.Append("create table ");
            sb.Append(table[0]);
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

            //defines the sql query command, and what database connection to execute it on
            SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);

            //runs command to build new table
            command.ExecuteNonQuery();

            //do clean up
            sb.Clear();
            m_dbConnection.Close();
        }

        /// <summary>
        /// Delete container presently in Main Window view from the database, if it exists therein.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        /// <param name="IndexToDelete"></param>
        /// <returns></returns>
        public static bool DeleteContainerFromProjectDatabase(string workingFolder, string dbFileName, long IndexToDelete)
        {
            //TODO when delete container is called figure out order of operations for deleting an entry out of the project sqlite database
            string[] table = dbFileName.Split('.');
            var fqFilePath = Path.Combine(workingFolder, dbFileName);

            //defines the SQLite connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

            //opens the connection
            m_dbConnection.Open();

            //defines index deletion command
            SQLiteCommand command = new SQLiteCommand("", m_dbConnection);

            //executes index deletion command; query returns # of rows affected, so affectedrows = 1 if index deleted successfully, 0 if not.
            int affectedRows = command.ExecuteNonQuery();

            //closes connection to database
            m_dbConnection.Close();

            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Populate the new project SQLite database file with standard fields plus whatever custom fields were added
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        /// <param name="specifiedCustomDBFieldsList"></param>
        /// <returns></returns>
        public static bool PopulateCustomProjectDatabase(string workingFolder, string dbFileName, ObservableCollection<IndividualDatabaseFieldModel> specifiedCustomDBFieldsList)
        {
            StringBuilder sb = new StringBuilder();
            string[] table = dbFileName.Split('.');
            var fqFilePath = Path.Combine(workingFolder, dbFileName);

            //defines the SQLite connection
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

            //opens the connection
            m_dbConnection.Open();

            //use stringbuider to build sql command.
            sb.Append("create table ");
            sb.Append(table[0]);
            sb.Append(" (ID INTEGER PRIMARY KEY AUTOINCREMENT,"); //index
            sb.Append(defaultFields[4] + " REAL,"); //xpos
            sb.Append(defaultFields[5] + " REAL,"); //ypos
            sb.Append(defaultFields[6] + " REAL,"); //zpos
            sb.Append(defaultFields[0] + " REAL,"); //length
            sb.Append(defaultFields[1] + " REAL,"); //width
            sb.Append(defaultFields[2] + " REAL,"); //height
            sb.Append(defaultFields[3] + " REAL,"); //weight            

            //appends custom fields, if there are any given
            if (specifiedCustomDBFieldsList.Count > 0)
            {
                sb.Append(defaultFields[7] + " VARCHAR(80),"); //name

                if (specifiedCustomDBFieldsList.Count == 1)
                {
                    sb.Append(specifiedCustomDBFieldsList[0].FieldName + " " + specifiedCustomDBFieldsList[0].FieldType);
                }
                else if (specifiedCustomDBFieldsList.Count == 2)
                {
                    sb.Append(specifiedCustomDBFieldsList[0].FieldName + " " + specifiedCustomDBFieldsList[0].FieldType + ",");
                    sb.Append(specifiedCustomDBFieldsList[1].FieldName + " " + specifiedCustomDBFieldsList[1].FieldType);
                }
                else
                {
                    for (int i = 0; i < specifiedCustomDBFieldsList.Count - 1; i++)
                    {
                        sb.Append(specifiedCustomDBFieldsList[i].FieldName + " " + specifiedCustomDBFieldsList[i].FieldType + ",");
                    }                    
                    sb.Append(specifiedCustomDBFieldsList[specifiedCustomDBFieldsList.Count - 1].FieldName + " " + specifiedCustomDBFieldsList[specifiedCustomDBFieldsList.Count - 1].FieldType);
                }
            } else
            {
                sb.Append(defaultFields[7] + " VARCHAR(80)"); //name
            }
            sb.Append(");");

            //defines the sql query command, and what database connection to execute it on
            SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);

            //runs command to build new table
            command.ExecuteNonQuery();

            //do clean up
            sb.Clear();
            m_dbConnection.Close();
            return false;
        }

        /// <summary>
        /// Clears out current contents of Database
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        /// 
        public static void ClearDatabase(string workingFolder, string dbFileName)
        {

            //split the file name into name plus extension
            string[] fileSplit = dbFileName.Split('.');
            //combine file path with file name to make a full file path for 
            var fqFilePath = Path.Combine(workingFolder, dbFileName);


            //if db file exists
            if (File.Exists(fqFilePath))
            {
                StringBuilder sb = new StringBuilder();
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");

                m_dbConnection.Open();

                sb.Append("delete from ");
                sb.Append(fileSplit[0]);
                sb.Append(");");
                sb.Clear(); //clear StringBuilder
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), m_dbConnection);
                
                command.ExecuteNonQuery(); //delete data

                m_dbConnection.Close();

            }
        }


        /// <summary>
        /// Clears out current contents of Database
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="CSVFileName">The CSV File that houses the current data</param>
        /// <param name="dbFileName">The DB File that has the DB info</param>
        /// 
        public static void UpdateDatabasefromCSV(string workingFolder, string CSVFileName, string dbFileName)
        {
            ObservableCollection<FullStackModel> modelList = Text.Reading.ReadText.ConvertCSVToStack(workingFolder, CSVFileName);

            ClearDatabase(workingFolder, dbFileName);

            List<string> oneLine = new List<string>();
            string[] items;

            foreach(var model in modelList)
            {
                items = model.ToString().Split(',');

                foreach(string str in items)
                {
                    oneLine.Add(str);
                }
                
                AddSQLiteData(workingFolder, dbFileName, oneLine);
                
            }
      
        }
        
        
        /// <summary>
        /// Update DB from CSV file
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="CSVFileName">The CSV File that houses the current data</param>
        /// <param name="dbFileName">The DB File that has the DB info</param>
        /// 
        public static void UpdateProjectDatabasefromCSV(string workingFolder, string CSVFileName, string dbFileName)
        {
            ObservableCollection<FullStackModel> modelList = Text.Reading.ReadText.ConvertCSVToStack(workingFolder, CSVFileName);

            ClearDatabase(workingFolder, dbFileName);

            List<string> oneLine = new List<string>();
            string[] items;

            foreach(var model in modelList)
            {
                items = model.ToString().Split(',');

                foreach(string str in items)
                {
                    oneLine.Add(str);
                }
                
                AddSQLiteData(workingFolder, dbFileName, oneLine);
                
            }
      
        }
        /// <summary>
        /// Updates Database based on FullStackModel provided
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="CSVFileName">The CSV File that houses the current data</param>
        /// <param name="dbFileName">The DB File that has the DB info</param>
        /// 
        public static void UpdateDatabasefromFullStackModel(string workingFolder, string dbFileName, ObservableCollection<FullStackModel> newModels)
        {

            //split the file name into name plus extension
            var fileSplit = dbFileName.Split('.');
            //combine file path with file name to make a full file path for 
            var fqFilePath = Path.Combine(workingFolder, dbFileName);

            //if db file exists
            if (File.Exists(fqFilePath))
            {
                StringBuilder UpdateQuery = new StringBuilder();
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + fqFilePath + ";Version=3;");
                SQLiteCommand updateCommand = new SQLiteCommand(m_dbConnection);
                List<long> IDsPresent = new List<long>();

                m_dbConnection.Open();

                //Go through the stack and update the position fields by creating U
                foreach (FullStackModel Stack in newModels)
                {
                    //start making a string list of all the IDs present (there probably is a better way to do this but TIME and all...)
                    IDsPresent.Add(Stack.Index);

                    //UPDATE tablename SET xpos = #, ypos = #, zpos = # WHERE ID = #;
                    UpdateQuery.Append("UPDATE " + fileSplit[0] + " SET xpos = " + Stack.XPOS + ", ypos = " + Stack.YPOS + ", zpos = " + Stack.ZPOS + " WHERE ID = " + Stack.Index + ";");

                    //Make the command text
                    updateCommand.CommandText = UpdateQuery.ToString();

                    //execute the command
                    updateCommand.ExecuteNonQuery();
                }

                UpdateQuery.Clear();

                //Go through the list and delete any containers that aren't in the stack
                //DELETE from tablename WHERE ID NOT IN (list of strings that are IDs);
                UpdateQuery.Append("DELETE from " + fileSplit[0] + " WHERE ID NOT IN (");
                foreach (long ID in IDsPresent)
                {
                    UpdateQuery.Append(ID);//adding the index to a list
                    if (IDsPresent.IndexOf(ID) != IDsPresent.Count - 1) UpdateQuery.Append(", "); //followed by a comma unless it's the last one
                }
                //Finish the line with );
                UpdateQuery.Append(");");

                //make command text
                updateCommand.CommandText = UpdateQuery.ToString();

                //Execute the DELETE command
                updateCommand.ExecuteNonQuery();

                //Clear StringBuilder
                UpdateQuery.Clear();
                //Close database connection
                m_dbConnection.Close();
            }

        }

        /// <summary>
        /// Writes a stack into the SQLite Database File provided
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dbFileName"></param>
        /// <param name="StackCollection"></param>
        public static void AddSQLiteDatafromStack(string workingFolder, string dbFileName, ObservableCollection<FullStackModel> StackCollection)
        {
            List<string> ListToAdd = new List<string>();

            foreach (FullStackModel stack in StackCollection)
            {
                ListToAdd.Add(stack.XPOS.ToString());
                ListToAdd.Add(stack.YPOS.ToString());
                ListToAdd.Add(stack.ZPOS.ToString());
                ListToAdd.Add(stack.Length.ToString());
                ListToAdd.Add(stack.Width.ToString());
                ListToAdd.Add(stack.Height.ToString());
                ListToAdd.Add(stack.Weight.ToString());
                ListToAdd.Add(stack.CrateName);
                AddSQLiteData(workingFolder, dbFileName, ListToAdd);
                ListToAdd.Clear();
            }
        }

    }
}
