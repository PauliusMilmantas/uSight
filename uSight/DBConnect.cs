using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uSight
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "usight";
            uid = "root";
            password = "";
            port = "3306";
            string connectionString;

            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, uid, password, database, 0);

            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
            } catch (Exception) {
                MessageBox.Show("Error: Can't connect to the internet", "Error", MessageBoxButtons.OK);
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Update DB
        public void updateWantedListDB() {
            if (connection != null)
            {
                DataExtraction e = new DataExtraction();

                dynamic obj = e.GetJsonFromDisk();

                MySqlCommand myCommand = new MySqlCommand("TRUNCATE TABLE wanted_list", connection);

                myCommand.ExecuteNonQuery();

                foreach (var json in obj.plates)
                {                 
                    myCommand = new MySqlCommand("INSERT INTO wanted_list (id, owner, license_plate, engine_number, vin) VALUES ('" + (String)json.id + "','" + (String)json.owner + "','" + (String)json.license_plate + "','" + (String)json.engine_number + "','" + (String)json.vin + "')", connection);
                    
                    myCommand.ExecuteNonQuery();
                }
            }
        }

        struct vehicles {
            string id;
            string plate_number;
            string vin;
            string owner;
        };

        public void updateWantedListJSON() {
            if (connection != null) {
                DataExtraction e = new DataExtraction();

                MySqlCommand myCommand = new MySqlCommand("SELECT * FROM wanted_list", connection);

                MySqlDataReader dataReader = myCommand.ExecuteReader();

                List<vehicles> vehicles;

                e.writeToJson("{\"plates\":[]}");
                dynamic json = e.GetJsonFromDisk();

                while (dataReader.Read()) {
                    JObject record = new JObject();

                    record["id"] = (String)dataReader["id"];
                    record["owner"] = (String)dataReader["owner"];
                    record["vin"] = (String)dataReader["vin"];
                    record["plate_number"] = (String)dataReader["license_plate"];

                    json.plates.Add(record);
                }

                e.writeToJson(json.ToString());
            }
        }

    }
}
