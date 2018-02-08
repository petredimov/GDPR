using Logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dac;
using DatabaseNamespace;

namespace Services
{
	/// <summary>
	/// Exporter class for handling pure database connection and backup
	/// </summary>
    public class Exporter
    {
        public string ConnectionString { get; set; }

        private string DatabaseName
        {
            get
            {
                if (connection != null)
                {
                    return connection.Database;
                }
                return String.Empty;
            }
        }

        SqlConnection connection = null;
        SqlCommand command = null;

        public Exporter(string connectionString)
        {
            ConnectionString = connectionString;

            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }

        ~Exporter()
        {
            if (connection != null)
            {				
				connection = null;
            }
        }

		/// <summary>
		/// Backup SQL windows hosted database
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
        public bool BackupDatabase(string filename)
        {
            try
            {
                string query = String.Format("BACKUP DATABASE {0} TO  DISK = '{1}'", DatabaseName, filename);
                command = new SqlCommand(query, connection);
                if (command.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
				Log.Write(ex.ToString(), LogLevel.Error, "");
            }
            
            return false;
        }


		/// <summary>
		/// Convert sql query to CSV file
		/// </summary>
		/// <param name="query"></param>
		/// <param name="filename"></param>
		/// <returns>boolean</returns>
        public bool SqlToCsv(string query, string filename)
        {
            try
            {
                command = new SqlCommand(query, connection);
                SqlDataReader dataReader = command.ExecuteReader();

                FileInfo info = new FileInfo(filename);

                if (info.Exists)
                {
                    info.Delete();
                }

                using (StreamWriter fs = new StreamWriter(info.FullName))
                {
                    // Loop through the fields and add headers
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        string name = dataReader.GetName(i);
                        if (name.Contains(","))
                            name = "\"" + name + "\"";

                        fs.Write(name + ",");
                    }

                    fs.WriteLine();

                    // Loop through the rows and output the data
                    while (dataReader.Read())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            string value = dataReader[i].ToString();
                            if (value.Contains(","))
                            {
                                value = "\"" + value + "\"";
                            }

                            fs.Write(value + ",");
                        }
                        fs.WriteLine();
                    }

                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Error in export to csv file", LogLevel.Error, string.Empty, ex);
            }
            return false;
        }		

		/// <summary>
		/// Export azure hosted SQL database in Azure
		/// </summary>
		/// <param name="backupPath"></param>
		public void ExportAzureDatabase(string backupPath)
		{
			SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
			csb.DataSource = DbCredentials.DbHost;
			csb.Password = DbCredentials.DbPassword;
			csb.UserID = DbCredentials.DbUser;

			DacServices ds = new DacServices(csb.ConnectionString);
			ds.ExportBacpac(backupPath, DbCredentials.DbName);
		}
	}
}
