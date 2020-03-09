using MySql.Data.MySqlClient;
using SAUSALibrary.FileHandling.XML.Reading;
using System;
using System.Data.SqlClient;
using System.IO;

namespace SAUSALibrary.FileHandling.Database.Reading
{
    public class ReadExternalDB
    {
        public static int ReadMySQLRecordCount(string[] dbparemeters, string workingFolder, string projectXMLFile)
        {
            string table;
            int count;

            var fqProjectFilePath = Path.Combine(workingFolder, projectXMLFile);

            if (File.Exists(fqProjectFilePath))
            {
                table = ReadXML.ReadProjectDBTableName(fqProjectFilePath);
            }
            else
            {
                throw new FileNotFoundException("Project file not found!");
            }

            MySqlConnectionStringBuilder dbConString = new MySqlConnectionStringBuilder
            {
                Server = dbparemeters[0],
                Database = dbparemeters[1],
                UserID = dbparemeters[2],
                Password = dbparemeters[3]
            };

            using (MySqlConnection connection = new MySqlConnection(dbConString.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string command = "SELECT COUNT(*) from " + table + ";";

                    using (MySqlCommand cmd = new MySqlCommand(command, connection))
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    //read data

                    return count;
                }
                catch (MySqlException)
                {
                    return -1;
                }
            }
        }

        public static int ReadMSSQLRecordCount(string[] dbparemeters, string workingFolder, string projectXMLFile)
        {
            string table;
            int count;

            var fqProjectFilePath = Path.Combine(workingFolder, projectXMLFile);

            if (File.Exists(fqProjectFilePath))
            {
                table = ReadXML.ReadProjectDBTableName(fqProjectFilePath);
            }
            else
            {
                throw new FileNotFoundException("Project file not found!");
            }

            SqlConnectionStringBuilder dbConString = new SqlConnectionStringBuilder
            {
                DataSource = dbparemeters[0],
                InitialCatalog = dbparemeters[1],
                UserID = dbparemeters[2],
                Password = dbparemeters[3]
            };

            using (SqlConnection connection = new SqlConnection(dbConString.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string command = "SELECT COUNT(*) from " + table + ";";

                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    //read data

                    return count;
                }
                catch (SqlException)
                {
                    return -1;
                }
            }
        }

        public static bool GetMySQLRecords(string[] dbparemeters, string workingFolder, string projectXMLFile, string projectSqLiteFile)
        {
            string table;

            var fqProjectFilePath = Path.Combine(workingFolder, projectXMLFile);
            var fqProjectDBFilePath = Path.Combine(workingFolder, projectSqLiteFile);

            if (File.Exists(fqProjectFilePath))
            {
                table = ReadXML.ReadProjectDBTableName(fqProjectFilePath);
            }
            else
            {
                throw new FileNotFoundException("Project file not found!");
            }

            MySqlConnectionStringBuilder dbConString = new MySqlConnectionStringBuilder
            {
                Server = dbparemeters[0],
                Database = dbparemeters[1],
                UserID = dbparemeters[2],
                Password = dbparemeters[3]
            };

            using (MySqlConnection connection = new MySqlConnection(dbConString.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string command = "SELECT * from " + table + ";";

                    using (MySqlCommand cmd = new MySqlCommand(command, connection))
                    {
                        //make a List to send to the SQL Lite add method
                        //List<string> list = new List<string>();

                        //populate the List
                        //TODO populate the list of read data from the database
                                                
                        if(File.Exists(fqProjectDBFilePath))
                        {
                            //finally add the new list to the project database
                            //WriteSQLite.AddSQLiteData(FilePathDefaults.ScratchFolder, projectSqLiteFile, list);
                        }
                        else
                        {
                            throw new FileNotFoundException("Project database to write new data into not found!");
                        }
                    }

                    //read data

                    return true;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }

        public static bool GetMSSQLRecords(string[] dbparemeters, string workingFolder, string projectXMLFile, string projectSqLiteFile)
        {
            string table;

            var fqProjectFilePath = Path.Combine(workingFolder, projectXMLFile);
            var fqProjectDBFilePath = Path.Combine(workingFolder, projectSqLiteFile);

            if (File.Exists(fqProjectFilePath))
            {
                table = ReadXML.ReadProjectDBTableName(fqProjectFilePath);
            }
            else
            {
                throw new FileNotFoundException("Project file not found!");
            }

            SqlConnectionStringBuilder dbConString = new SqlConnectionStringBuilder
            {
                DataSource = dbparemeters[0],
                InitialCatalog = dbparemeters[1],
                UserID = dbparemeters[2],
                Password = dbparemeters[3]
            };

            using (SqlConnection connection = new SqlConnection(dbConString.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string command = "SELECT * from " + table + ";";

                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    {
                        //make a List to send to the SQL Lite add method
                        //List<string> list = new List<string>();

                        //populate the List
                        //TODO populate the list of read data from the database
                                                
                        if (File.Exists(fqProjectDBFilePath))
                        {
                            //finally add the new list to the project database
                            //WriteSQLite.AddSQLiteData(FilePathDefaults.ScratchFolder, projectSqLiteFile, list);
                        }
                        else
                        {
                            throw new FileNotFoundException("Project database to write new data into not found!");
                        }
                    }

                    //read data

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}
