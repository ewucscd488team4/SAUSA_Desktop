using MySql.Data.MySqlClient;
using System.Data;

namespace SAUSALibrary.FileHandling.Database.Writing
{
    public class WriteExternalDB
    {
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
            bool returnMe = false;
            try
            {
                using (conn = new MySqlConnection(dbConString.ConnectionString))
                {
                    conn.Open();
                    returnMe = true;
                }

            }
            catch (MySqlException)
            {
                return returnMe;
            }
            return returnMe;
        }

        public static bool TestMSSQL_DBConnection(string[] testParameters)
        {
            return false;
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
    }
}
