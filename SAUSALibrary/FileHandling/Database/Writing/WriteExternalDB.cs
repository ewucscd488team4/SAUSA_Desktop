using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace SAUSALibrary.FileHandling.Database.Writing
{
    public class WriteExternalDB
    {
        /// <summary>
        /// Tests the given mysql connection using the paremeters given in the parameters array
        /// </summary>
        /// <param name="testParameters"></param>
        /// <returns></returns>
        public static bool TestMySQL_DBConnection(string[] testParameters)
        {
            MySqlConnection conn;
            MySqlConnectionStringBuilder dbConString = new MySqlConnectionStringBuilder
            {
                Server = testParameters[0],
                Database = testParameters[1],
                UserID = testParameters[2],
                Password = testParameters[3]
            };
            
            try
            {
                using (conn = new MySqlConnection(dbConString.ConnectionString))
                {
                    conn.Open();
                    return true;
                }

            }
            catch (MySqlException)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Tests the given sql connection using the paremeters given in the paremeters array
        /// </summary>
        /// <param name="testParameters"></param>
        /// <returns></returns>
        public static bool TestMSSQL_DBConnection(string[] testParameters)
        {
            SqlConnectionStringBuilder dbConString = new SqlConnectionStringBuilder
            {
                DataSource = testParameters[0],
                InitialCatalog = testParameters[1],
                UserID = testParameters[2],
                Password = testParameters[3]
            };
            using (SqlConnection connection = new SqlConnection(dbConString.ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

        }

        public static bool SetUpMySQLDatabase(string[] dbparemeters)
        {
            /*
             CREATE TABLE TestData (
            crateIndex smallint(4) NOT NULL AUTO_INCREMENT,
            xPos Decimal(10,4),
            yPos Decimal(10,4),
            zPos Decimal(10,4),
            length Decimal(8,4) NOT NULL,
            width Decimal(8,4) NOT NULL,
            height Decimal(8,4) NOT NULL,
            weight Decimal(7,2) NOT NULL,
            name VARCHAR(150) NOT NULL,
            PRIMARY KEY (crateIndex)
             */
            return false;
        }

        public static bool SetUpSQLDatabase(string[] dbparameters)
        {
            return false;
        }

        public static void ExportProjectToSQL()
        {

        }

        public static void ExportProjectToMySQL()
        {

        }
    }
}
